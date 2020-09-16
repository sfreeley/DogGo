﻿using System;
using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, NeighborhoodId, Phone
                        FROM Owner
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //cmd.CommandText = @"
                    //    SELECT o.Id, 
                    //    o.[Name], 
                    //    o.Email, 
                    //    o.[Address], 
                    //    o.NeighborhoodId, 
                    //    o.Phone,
                    //    d.Id,
                    //    d.[Name],
                    //    d.Breed,
                    //    d.Notes,
                    //    d.ImageUrl,
                    //    d.OwnerId
                    //    FROM Owner o
                    //    JOIN Dog d ON d.OwnerId = o.Id
                    //    WHERE Id = @id
                    //";

                    cmd.Parameters.AddWithValue("@id", id);
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        //Owner owner = null;
                        //Dog dog = new Dog()
                        //{
                        //    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        //    Name = reader.GetString(reader.GetOrdinal("Name")),
                        //    Breed = reader.GetString(reader.GetOrdinal("Breed")),
                        //    Notes = reader.GetString(reader.GetOrdinal("Notes")),
                        //    ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                        //    OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                        Owner owner = new Owner
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone"))
                        };
                       
                        reader.Close();
                        return owner;
                    }





                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
    }
}

