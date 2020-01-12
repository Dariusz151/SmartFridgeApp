using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartFridgeApp.Domain.FoodProducts;
using SmartFridgeApp.Domain.Fridges;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.Fridges;

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

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(PaymentCreatedNotification).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IDomainEventNotification<>)).InstancePerDependency();

            //builder.RegisterGenericDecorator(
            //    typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            //    typeof(INotificationHandler<>));

            //builder.RegisterGenericDecorator(
            //    typeof(DomainEventsDispatcherCommandHandlerDecorator<>),
            //    typeof(IRequestHandler<,>));

            //builder.RegisterType<CommandsDispatcher>()
            //    .As<ICommandsDispatcher>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<CommandsScheduler>()
            //    .As<ICommandsScheduler>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<StronglyTypedIdValueConverterSelector>()
            //    .As<IValueConverterSelector>()
            //    .InstancePerLifetimeScope();
        }
    }
}
