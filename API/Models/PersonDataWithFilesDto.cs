﻿namespace DropSpace.API.Models
{
    public class PersonDataWithFilesDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public int? UnionId { get; set; }
        public string? UnionName { get; set; }
        public int? VillageId { get; set; }
        public string? VillageName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public List<UploadedFileDto>? UploadedFiles { get; set; }
    }

    public class UploadedFileDto
    {
        public int Id { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}