using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;


namespace SearchService.Entities;

public class User: Entity
{
    public string UserId {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string NormalizedName {get; set;}
    public string Email {get; set;}
    public string UserType {get; set;}
}
