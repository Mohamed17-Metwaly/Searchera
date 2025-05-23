﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
#nullable disable

namespace Searchera.Models;

public partial class JobBoardSystemContext : DbContext
{
    public JobBoardSystemContext() : base()
    {
    }

    public JobBoardSystemContext(DbContextOptions<JobBoardSystemContext> options) : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<JobListing> JobListings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }  

}
