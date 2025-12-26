using AutoMapper;
using EduAdmin.Entities;
using EduAdmin.LocalTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduAdmin.FileManagements.Dto
{
    public class FileManagementMapProfile : Profile
    {
        public FileManagementMapProfile()
        {
            CreateMap<FileCreateDto, FileManagement>().AfterMap((source, dto) =>
            {
                if (string.IsNullOrEmpty(source.RelativePath))
                {
                
                }
                dto.IsDeleted = false;
            });
            CreateMap<FileManagement, FileCreateDto>().AfterMap((source, dto) =>
            {
            });
           
            CreateMap<FileManagement, FileShortOutputDto>().AfterMap((source, dto) =>
            {
                dto.FileId = source.Id.ToString();
                dto.Url = source.RelativePath;
                dto.Creator = source.CreatorUserId;
                dto.Description = source.Description;
                dto.IsProject = false;
                dto.Type = LocalTool.FileTypeTFM(source.Type);
                dto.LastTime = source.LastModificationTime == null ?
                    LocalTool.TimeFormatStr(source.CreationTime,0): 
                    LocalTool.TimeFormatStr((DateTime)source.LastModificationTime, 0);
            });
            CreateMap<FileManagement, FileOutputDto>().AfterMap((source, dto) =>
            {
                dto.Url = source.RelativePath;
                dto.Type = LocalTool.FileTypeTFM(source.Type);
                dto.LastTime = source.LastModificationTime == null ?
                    LocalTool.TimeFormatStr(source.CreationTime, 0) :
                    LocalTool.TimeFormatStr((DateTime)source.LastModificationTime, 0);
            });
            
        }
    }
}
