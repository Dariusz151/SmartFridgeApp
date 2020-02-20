using Autofac;
using SmartFridgeApp.API.Recipes;
using SmartFridgeApp.Domain.DomainServices;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.Recipes;
using SmartFridgeApp.Domain.SeedWork;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Infrastructure.FoodProducts;
using SmartFridgeApp.Infrastructure.Fridges;
using SmartFridgeApp.Infrastructure.Recipes;

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

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();
        }
    }
}
