﻿using System.Reflection;
using Autofac;
using MediatR;
using SmartFridgeApp.API.Notifications;
using SmartFridgeApp.API.Notifications.Fridge;
using SmartFridgeApp.API.Notifications.Recipe;
using SmartFridgeApp.API.Notifications.Users;
using SmartFridgeApp.API.Services;
using SmartFridgeApp.Domain.DomainServices;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.Fridges;
using SmartFridgeApp.Infrastructure.Recipes;
using SmartFridgeApp.Infrastructure.SeedWork;

namespace SmartFridgeApp.API.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly string _dbConnectionString;

        public InfrastructureModule(string dbConnectionString)
        {
            this._dbConnectionString = dbConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _dbConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<FridgeRepository>()
                .As<IFridgeRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FoodProductRepository>()
                .As<IFoodProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RecipeRepository>()
                .As<IRecipeRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RecipeFinderService>()
                .As<IRecipeFinderService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailSender>()
                .As<INotifier>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(FridgeAddedNotification).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(RecipeAddedNotification).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(UserAddedNotification).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();
        }
    }
}
