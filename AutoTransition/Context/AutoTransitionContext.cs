using AutoTransition.Context.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoTransition.Context
{
    public class AutoTransitionContext:DbContext
    {
        public AutoTransitionContext():base("AutoTransitionConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AutoRoute> AutoRoutes { get; set; }
        public DbSet<TransportationTypes> TransportationTypes { get; set; }
        public DbSet<CargoDimensions> CargoDimensions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Record> Records { get; set; }
    }
}