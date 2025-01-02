using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace SearchService.Entities;

public class Form: Entity
{
    public string FormTemplateId {get; set;}

    public string Title {get; set;}
    public string Description{get; set;}
    public string BannerUrl {get; set;}
}
