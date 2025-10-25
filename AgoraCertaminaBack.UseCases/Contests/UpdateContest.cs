using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Services.Interfaces;
using AgoraCertaminaBack.UseCases.Shared;
using MongoDB.Bson;
using ROP;
using System;
using System.Text.Json;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class UpdateContest(IMongoRepository<Contest> _mongoRepository, UserRequestContext _userRequest, IFileManager _fileManager, GetByIdContest _getByIdContest )
    {
        internal static readonly HashSet<int> fileTypes = [(int)FieldTypeEnum.Image, (int)FieldTypeEnum.Archive];
        private static readonly HashSet<string> EmptyValues = new(StringComparer.OrdinalIgnoreCase)
        {
            "", "null", "BsonNull", "bjsonnull", "undefined"
        };

        public async Task<Result<string>> Execute(string contestId, ContestUpdateRequest updateRequest)
        {
            var validated = ValidateUpdateRequest(updateRequest);
            if (!validated.Success)
                return Result.Failure<string>(validated.ToString());

            var fetchedContest = await _getByIdContest.Execute(contestId);
            if (!fetchedContest.Success)
                return Result.Failure<string>(fetchedContest.ToString());

            var permissionChecked = CheckCustomerPermission(fetchedContest.Value);
            if (!permissionChecked.Success)
                return Result.Failure<string>(permissionChecked.ToString());


            var result = await UpdateContestAsync(permissionChecked.Value, updateRequest);

            return result;
        }

        private static Result<ContestUpdateRequest> ValidateUpdateRequest(ContestUpdateRequest updateRequest)
        {
            if (updateRequest == null)
                return Result.Failure<ContestUpdateRequest>("Update request cannot be null");

            if (updateRequest.Fields?.Any(f => string.IsNullOrEmpty(f.Id)) == true)
                return Result.Failure<ContestUpdateRequest>("All fields must have a valid ID");

            if (updateRequest.Fields?.Any(f => string.IsNullOrEmpty(f.Name)) == true)
                return Result.Failure<ContestUpdateRequest>("All fields must have a valid name");

            return updateRequest.Success();
        }

        private Result<Contest> CheckCustomerPermission(Contest contest)
        {
            return contest.OrganizationId != _userRequest.OrganizationId
                ? Result.Failure<Contest>("Uneditable contest")
                : contest.Success();
        }

        private async Task<Result<string>> UpdateContestAsync(Contest contest, ContestUpdateRequest updateRequest)
        {
            try
            {
                var result = await UpdateFieldsAsync(contest, updateRequest.Fields)
                    .Bind(_ => UpdateTags(contest, updateRequest.Tags))
                    .Bind(_ => SaveContestAsync(contest));

                return result;
            }
            catch (JsonException ex)
            {
                return Result.Failure<string>($"Invalid JSON in field value: {ex.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                return Result.Failure<string>("Insufficient permissions to update contest");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error updating contest: {ex.Message}");
            }
        }

        private async Task<Result<string>> UpdateFieldsAsync(Contest contest, List<FieldDTO>? fields)
        {
            if (fields == null || fields.Count == 0)
                return Result.Success("No fields to update");

            try
            {
                foreach (var updatedField in fields)
                {
                    var result = await ProcessFieldUpdate(contest, updatedField);

                    if (!result.Success)
                        return Result.Failure<string>("There was a problem updating the field.");
                }

                return Result.Success("Fields updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error updating fields: {ex.Message}");
            }
        }

        private async Task<Result<string>> ProcessFieldUpdate(Contest contest, FieldDTO updatedField)
        {
            try
            {
                var existingField = contest.Fields.Find(f => f.Id == updatedField.Id);

                // Validar si el campo es de archivo y está vacío/nulo
                if (IsFileType(updatedField.Type) &&
                    (string.IsNullOrWhiteSpace(updatedField.Value) || EmptyValues.Contains(updatedField.Value.Trim())))
                {
                    if (existingField != null)
                    {
                        // Eliminar archivos antiguos solo si existen
                        if (ShouldDeleteOldFiles(existingField))
                        {
                            string oldValue = GetStringFromBsonValue(existingField.Value);
                            await DeleteOldFilesAsync(oldValue);
                        }
                        existingField.Value = BsonNull.Value;
                    }
                    return Result.Success("Field updated to empty");
                }

                if (existingField != null)
                {
                    await UpdateExistingField(existingField, updatedField, contest);
                }
                else
                {
                    await CreateNewField(contest, updatedField);
                }

                return Result.Success("Success");
            }
            catch (Exception ex)
            {
                // Error específico por campo sin afectar otros
                return Result.Failure<string>($"Error processing field {updatedField.Name}: {ex.Message}");
            }
        }

        private async Task UpdateExistingField(FieldValue existingField, FieldDTO updatedField, Contest contest)
        {
            existingField.FieldName = updatedField.Name;
            string newValue = await ProcessFieldValueAsync(contest, updatedField);
            existingField.Value = ParseBsonValueByType(updatedField.Type, newValue);
            existingField.Type = updatedField.Type;
        }

        private async Task CreateNewField(Contest contest, FieldDTO updatedField)
        {
            var newField = new FieldValue
            {
                Id = updatedField.Id,
                FieldName = updatedField.Name,
                Value = BsonValue.Create(await ProcessFieldValueAsync(contest, updatedField)),
                Type = updatedField.Type,
                IsBase = updatedField.IsBase
            };

            contest.Fields.Add(newField);
        }

        private static Result<string> UpdateTags(Contest contest, List<CustomTagDTO>? tags)
        {
            if (tags == null)
                return Result.Success("No tags to update");

            try
            {
                contest.Tags = tags.Select(tag => new Tag
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color
                }).ToList();

                return Result.Success("Tags updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error updating tags: {ex.Message}");
            }
        }

        private async Task<Result<string>> SaveContestAsync(Contest contest)
        {
            try
            {
                await _mongoRepository.ReplaceOneAsync(contest);
                return contest.Id.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure<string>($"Error saving contest: {ex.Message}");
            }
        }

        private async Task<string> ProcessFieldValueAsync(Contest contest, FieldDTO field)
        {
            if (!IsFileType(field.Type))
            {
                return field.Value ?? string.Empty;
            }

            // Validar si el valor está vacío o es un valor nulo conocido
            if (string.IsNullOrWhiteSpace(field.Value) || EmptyValues.Contains(field.Value.Trim()))
            {
                return string.Empty;
            }

            try
            {
                var files = DeserializeFiles(field.Value);
                if (files == null || !files.Any())
                {
                    return string.Empty;
                }

                return await UploadFilesWithRollbackAsync(contest, files);
            }
            catch (JsonException)
            {
                // Si no se puede deserializar, asumimos que no es un archivo válido
                return string.Empty;
            }
        }

        private async Task<string> UploadFilesWithRollbackAsync(Contest contest, IEnumerable<FieldFileRequest> files)
        {
            var uploadedPaths = new List<string>();
            string pathBase = BuildPathBase(contest);

            foreach (var file in files)
            {
                try
                {
                    string savePath = $"{pathBase}/{Guid.NewGuid()}_{file.Name}"; // Agregar GUID para evitar colisiones
                    bool uploaded = await _fileManager.UploadFileAsync(savePath, file.ContentStream);

                    if (!uploaded)
                    {
                        // Solo borramos los archivos subidos hasta ahora para este campo
                        await DeleteFilesAsync(uploadedPaths);
                        return string.Empty;
                    }

                    uploadedPaths.Add(savePath);
                }
                catch
                {
                    await DeleteFilesAsync(uploadedPaths);
                    throw;
                }
            }

            return uploadedPaths.Count > 0 ? string.Join(",", uploadedPaths) : string.Empty;
        }

        private static string BuildPathBase(Contest contest)
        {
            return $"{contest.CustomerName}/{contest.SchemaName}/{contest.ReferenceNumber}";
        }

        private static IEnumerable<FieldFileRequest>? DeserializeFiles(string fieldValue)
        {
            try
            {
                return JsonSerializer.Deserialize<IEnumerable<FieldFileRequest>>(fieldValue);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static bool IsFileType(FieldTypeEnum type)
        {
            return fileTypes.Contains((int)type);
        }

        private static bool ShouldDeleteOldFiles(FieldValue existingField)
        {
            return IsFileType(existingField.Type) && HasValidStringValue(existingField.Value);
        }

        private async Task DeleteOldFilesAsync(string filePaths)
        {
            if (string.IsNullOrEmpty(filePaths)) return;

            var paths = filePaths.Split(',', StringSplitOptions.RemoveEmptyEntries);
            await DeleteFilesAsync(paths);
        }

        private async Task DeleteFilesAsync(IEnumerable<string> filePaths)
        {
            var deleteTasks = filePaths.Select(async path =>
            {
                try
                {
                    await _fileManager.DeleteFileAsync(path.Trim());
                }
                catch (Exception)
                { }
            });

            await Task.WhenAll(deleteTasks);
        }

        private static string GetStringFromBsonValue(BsonValue? bsonValue)
        {
            if (bsonValue == null || bsonValue.IsBsonNull)
                return string.Empty;

            return bsonValue.IsString ? bsonValue.AsString : bsonValue.ToString() ?? string.Empty;
        }

        private static bool HasValidStringValue(BsonValue? bsonValue)
        {
            if (bsonValue == null || bsonValue.IsBsonNull)
                return false;

            string? stringValue = bsonValue.IsString ? bsonValue.AsString : bsonValue.ToString();
            return !string.IsNullOrEmpty(stringValue);
        }

        private static BsonValue ParseBsonValueByType(FieldTypeEnum type, string value)
        {
            if (string.IsNullOrEmpty(value))
                return BsonNull.Value;

            return type switch
            {
                FieldTypeEnum.Boolean => ParseBooleanValue(value),
                FieldTypeEnum.Integer => ParseNumericValue(value),
                FieldTypeEnum.Date => ParseDateValue(value),
                FieldTypeEnum.Image or FieldTypeEnum.Archive => BsonValue.Create(value),
                _ => BsonValue.Create(value),
            };
        }

        private static BsonValue ParseBooleanValue(string value)
        {
            return bool.TryParse(value, out var b) ? BsonValue.Create(b) : BsonNull.Value;
        }

        private static BsonValue ParseNumericValue(string value)
        {
            return double.TryParse(value, out var n) ? BsonValue.Create(n) : BsonNull.Value;
        }

        private static BsonValue ParseDateValue(string value)
        {
            return DateTime.TryParse(value, out var d) ? BsonValue.Create(d) : BsonNull.Value;
        }

    }
}