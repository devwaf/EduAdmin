using AutoMapper;
using EduAdmin.Entities;
using EduAdmin.FileManagements.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.AppService.GraduationDesigns.Dto
{
    public class GraduationDesignMapFileDto : Profile
    {
        public GraduationDesignMapFileDto()
        {
            CreateMap<CreateGraduationDesignDto, GraduationDesign>().AfterMap((dto, sourse) =>
            {
                if (sourse.Annex != null)
                    sourse.Annex = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.Annex
                });
                if (sourse.SecondReport != null)
                    sourse.SecondReport = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.SecondReport
                });
                if (sourse.Assignment != null)
                    sourse.Assignment = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.Assignment
                });
                if (sourse.CheckReport != null)
                    sourse.CheckReport = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.CheckReport
                });
                if (sourse.Dissertation != null)
                    sourse.Dissertation = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.Dissertation
                });
                if (sourse.Headline != null)
                    sourse.Headline = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.Headline
                });
                if (sourse.FirstReport != null)
                    sourse.FirstReport = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.FirstReport
                });
                if (sourse.ForeignTrans != null)
                    sourse.ForeignTrans = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.ForeignTrans
                });
                if (sourse.DraftDissertation != null)
                    sourse.DraftDissertation = JsonConvert.SerializeObject(new GraDsignFileAndState
                {
                    State = true,
                    FilePath = dto.DraftDissertation
                });
            });
            CreateMap<GraduationDesign, GraduationDesignShowDto>().AfterMap((sourse, dto) =>
            {
                if (sourse.Annex != null) {
                    var annrx = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Annex);
                    dto.Annex = new ShowGraFileDto
                    {
                        State = annrx.State,
                        Url = annrx.FilePath.Path
                    };
                }
                if (sourse.SecondReport != null)
                {
                    var secondReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.SecondReport);
                    dto.SecondReport = new ShowGraFileDto
                    {
                        State = secondReport.State,
                        Url = secondReport.FilePath.Path
                    };
                }
                if (sourse.Assignment != null)
                {
                    var assignment = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Assignment);
                    dto.Assignment = new ShowGraFileDto
                    {
                        State = assignment.State,
                        Url = assignment.FilePath.Path
                    };
                }
                if (sourse.CheckReport != null)
                {
                    var checkReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.CheckReport);
                    dto.CheckReport = new ShowGraFileDto
                    {
                        State = checkReport.State,
                        Url = checkReport.FilePath.Path
                    };
                }
                if (sourse.Dissertation != null)
                {
                    var dissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Dissertation);
                    dto.Dissertation = new ShowGraFileDto
                    {
                        State = dissertation.State,
                        Url = dissertation.FilePath.Path
                    };
                }
                if (sourse.Headline != null)
                {
                    var headline = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Headline);
                    dto.Headline = new ShowGraFileDto
                    {
                        State = headline.State,
                        Url = headline.FilePath.Path
                    };
                }
                if (sourse.FirstReport != null)
                {
                    var firstReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.FirstReport);
                    dto.FirstReport = new ShowGraFileDto
                    {
                        State = firstReport.State,
                        Url = firstReport.FilePath.Path
                    };
                }
                if (sourse.ForeignTrans != null)
                {
                    var foreignTrans = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.ForeignTrans);
                    dto.ForeignTrans = new ShowGraFileDto
                    {
                        State = foreignTrans.State,
                        Url = foreignTrans.FilePath.Path
                    };
                }
                if (sourse.DraftDissertation != null)
                {
                    var draftDissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.DraftDissertation);
                    dto.DraftDissertation = new ShowGraFileDto
                    {
                        State = draftDissertation.State,
                        Url = draftDissertation.FilePath.Path
                    };
                }
            });
        CreateMap<GraduationDesign, GraduationDesignStuShowDto>().AfterMap((sourse, dto) =>
            {
            if (sourse.Annex != null)
            {
                var annrx = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Annex);
                dto.Annex = new ShowGraFileDto
                {
                    State = annrx.State,
                    Url = annrx.FilePath.Path
                };
            }
            if (sourse.SecondReport != null)
            {
                var secondReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.SecondReport);
                dto.SecondReport = new ShowGraFileDto
                {
                    State = secondReport.State,
                    Url = secondReport.FilePath.Path
                };
            }
            if (sourse.Assignment != null)
            {
                var assignment = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Assignment);
                dto.Assignment = new ShowGraFileDto
                {
                    State = assignment.State,
                    Url = assignment.FilePath.Path
                };
            }
            if (sourse.CheckReport != null)
            {
                var checkReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.CheckReport);
                dto.CheckReport = new ShowGraFileDto
                {
                    State = checkReport.State,
                    Url = checkReport.FilePath.Path
                };
            }
            if (sourse.Dissertation != null)
            {
                var dissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Dissertation);
                dto.Dissertation = new ShowGraFileDto
                {
                    State = dissertation.State,
                    Url = dissertation.FilePath.Path
                };
            }
            if (sourse.Headline != null)
            {
                var headline = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.Headline);
                dto.Headline = new ShowGraFileDto
                {
                    State = headline.State,
                    Url = headline.FilePath.Path
                };
            }
            if (sourse.FirstReport != null)
            {
                var firstReport = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.FirstReport);
                dto.FirstReport = new ShowGraFileDto
                {
                    State = firstReport.State,
                    Url = firstReport.FilePath.Path
                };
            }
            if (sourse.ForeignTrans != null)
            {
                var foreignTrans = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.ForeignTrans);
                dto.ForeignTrans = new ShowGraFileDto
                {
                    State = foreignTrans.State,
                    Url = foreignTrans.FilePath.Path
                };
            }
            if (sourse.DraftDissertation != null)
            {
                var draftDissertation = JsonConvert.DeserializeObject<GraDsignFileAndState>(sourse.DraftDissertation);
                dto.DraftDissertation = new ShowGraFileDto
                {
                    State = draftDissertation.State,
                    Url = draftDissertation.FilePath.Path
                };
            }
        });
        }
}
}
