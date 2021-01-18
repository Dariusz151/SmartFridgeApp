using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;
using System.Threading;
using System.Collections.Generic;
using SmartFridgeApp.Infrastructure;
using SmartFridgeApp.Domain.Models.Recipes;
using System.Linq;
using SmartFridgeApp.Domain.Shared;
using System;
using SmartFridgeApp.Domain.Models.Fridges;
using SmartFridgeApp.Domain.Models.FridgeItems;
using SmartFridgeApp.Domain.Models.Users;

namespace SmartFridgeApp.API
{
    
    [ApiController]
    public class InitializationController : Controller
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly SmartFridgeAppContext _context;
        //private readonly IUnitOfWork _unitOfWork;
        private static List<FoodProduct> foodProducts;
        RecipeCategory categorySniadanie;
        RecipeCategory categoryObiad;
        RecipeCategory categoryKolacja;

        public InitializationController(
            IUnitOfWork unitOfWork, IFoodProductRepository foodProductRepository, SmartFridgeAppContext context)
        {
            //_unitOfWork = unitOfWork;
            _foodProductRepository = foodProductRepository;
            _context = context;
            foodProducts = new List<FoodProduct>();

            categorySniadanie = new RecipeCategory("Śniadanie");
            categoryObiad = new RecipeCategory("Obiad");
            categoryKolacja = new RecipeCategory("Kolacja");
        }

        /// <summary>
        /// Init FoodProduct and Fridges Database. This class is just for testing and development purposes (only for admin)!!
        /// </summary>
        [Route("api/init/fridges")]
        [HttpPost]
        [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitFoodProductFridgesDatabase()
        {
            var foodProducts = GenerateFoodProducts();
            foreach (var fp in foodProducts)
            {
                await _context.FoodProducts.AddAsync(fp);
            }

            var fridges = GenerateFridges();
            foreach (var fridge in fridges)
            {
                await _context.Fridges.AddAsync(fridge);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Init Recipes Database. This class is just for testing and development purposes (only for admin)!!
        /// </summary>
        [Route("api/init/recipes")]
        [HttpPost]
        [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitRecipesDatabase()
        {
            var dbfoodProducts = await _foodProductRepository.GetAllAsync();
            var recipes = GenerateRecipes(dbfoodProducts);

            foreach (var recipe in recipes){
                await _context.Recipes.AddAsync(recipe);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private IEnumerable<Fridge> GenerateFridges()
        {
            var fridges = new List<Fridge>();

            var user1 = new User("Dariusz", "dariusz@dariusz.pl");
            var user2 = new User("Olga", "olga@olga.pl");
            var user3 = new User("Zbychu", "zbychu@elk.pl");
            var user4 = new User("Krzysztof", "krzysztof@elk.pl");
            var user5 = new User("Pietrek", "pietruj@elk.pl");

            var user1FridgeItems = new List<FridgeItem>();
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Pierś z kurczaka")).SingleOrDefault(), "Z biedry", new AmountValue(500, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ryż biały")).SingleOrDefault(), "Z lidla", new AmountValue(400, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Sos słodko-kwaśny")).SingleOrDefault(), "łowicz", new AmountValue(500, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Masło")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Cukinia")).SingleOrDefault(), "", new AmountValue(1, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Cebula")).SingleOrDefault(), "", new AmountValue(15, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Czosnek")).SingleOrDefault(), "", new AmountValue(10, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(), "", new AmountValue(10, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Twaróg")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Papryka")).SingleOrDefault(), "", new AmountValue(1, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Pomidor")).SingleOrDefault(), "", new AmountValue(5, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Cytryna")).SingleOrDefault(), "", new AmountValue(3, Unit.Pieces)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ser żółty")).SingleOrDefault(), "", new AmountValue(200, Unit.Grams)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(), "", new AmountValue(1000, Unit.Mililiter)));
            user1.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Wino")).SingleOrDefault(), "", new AmountValue(500, Unit.Mililiter)));

            var user2FridgeItems = new List<FridgeItem>();
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Pierś z kurczaka")).SingleOrDefault(), "Z biedry", new AmountValue(500, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Sos słodko-kwaśny")).SingleOrDefault(), "łowicz", new AmountValue(500, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Masło")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Cukinia")).SingleOrDefault(), "", new AmountValue(1, Unit.Pieces)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Cebula")).SingleOrDefault(), "", new AmountValue(15, Unit.Pieces)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Czosnek")).SingleOrDefault(), "trzeba go zjeść", new AmountValue(10, Unit.Pieces)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(), "z wioski", new AmountValue(10, Unit.Pieces)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Dorsz")).SingleOrDefault(), "filet", new AmountValue(250, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(), "", new AmountValue(500, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Pomidor")).SingleOrDefault(), "", new AmountValue(5, Unit.Pieces)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ryż basmati")).SingleOrDefault(), "", new AmountValue(400, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Makaron spaghetti")).SingleOrDefault(), "", new AmountValue(600, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Natka pietruszki")).SingleOrDefault(), "zamrożona", new AmountValue(600, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ser żółty")).SingleOrDefault(), "", new AmountValue(200, Unit.Grams)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(), "mlekowita ", new AmountValue(1000, Unit.Mililiter)));
            user2.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ziemniaki")).SingleOrDefault(), "", new AmountValue(500, Unit.Grams)));

            var user3FridgeItems = new List<FridgeItem>();
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Piwo")).SingleOrDefault(), "harnaś", new AmountValue(500, Unit.Mililiter)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Sos słodko-kwaśny")).SingleOrDefault(), "łowicz", new AmountValue(500, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Masło")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Bułki")).SingleOrDefault(), "", new AmountValue(5, Unit.Pieces)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Margaryna")).SingleOrDefault(), "", new AmountValue(200, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Czosnek")).SingleOrDefault(), "", new AmountValue(10, Unit.Pieces)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(), "", new AmountValue(10, Unit.Pieces)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(), "", new AmountValue(500, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ser żółty")).SingleOrDefault(), "gouda", new AmountValue(200, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(), "", new AmountValue(1000, Unit.Mililiter)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Por")).SingleOrDefault(), "", new AmountValue(1, Unit.Pieces)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Marchew")).SingleOrDefault(), "", new AmountValue(3, Unit.Pieces)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mięso mielone wieprzowe")).SingleOrDefault(), "z promki było dużo", new AmountValue(750, Unit.Grams)));
            user3.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Kiełbasa")).SingleOrDefault(), "podwawelska", new AmountValue(3, Unit.Pieces)));

            var user4FridgeItems = new List<FridgeItem>();
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Piwo")).SingleOrDefault(), "harnaś", new AmountValue(500, Unit.Mililiter)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Sos słodko-kwaśny")).SingleOrDefault(), "łowicz", new AmountValue(500, Unit.Grams)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Masło")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Bułki")).SingleOrDefault(), "", new AmountValue(5, Unit.Pieces)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ser żółty")).SingleOrDefault(), "gouda", new AmountValue(200, Unit.Grams)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(), "", new AmountValue(1000, Unit.Mililiter)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Por")).SingleOrDefault(), "", new AmountValue(1, Unit.Pieces)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mięso mielone wieprzowe")).SingleOrDefault(), "z promki było dużo", new AmountValue(750, Unit.Grams)));
            user4.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Kiełbasa")).SingleOrDefault(), "podwawelska", new AmountValue(3, Unit.Pieces)));

            var user5FridgeItems = new List<FridgeItem>();
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Piwo")).SingleOrDefault(), "harnaś", new AmountValue(500, Unit.Mililiter)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Masło")).SingleOrDefault(), "", new AmountValue(250, Unit.Grams)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Margaryna")).SingleOrDefault(), "", new AmountValue(200, Unit.Grams)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(), "", new AmountValue(500, Unit.Grams)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Ser żółty")).SingleOrDefault(), "gouda", new AmountValue(200, Unit.Grams)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(), "", new AmountValue(1000, Unit.Mililiter)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Marchew")).SingleOrDefault(), "", new AmountValue(3, Unit.Pieces)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Mięso mielone wieprzowe")).SingleOrDefault(), "z promki było dużo", new AmountValue(750, Unit.Grams)));
            user5.AddFridgeItem(new FridgeItem(foodProducts.Where(x => x.Name.Equals("Kiełbasa")).SingleOrDefault(), "podwawelska", new AmountValue(3, Unit.Pieces)));


            var fridgeDragana = new Fridge("Dragana", "Dragana", "Dragana");
            fridgeDragana.AddUser(user1);
            fridgeDragana.AddUser(user2);

            var fridgeElk = new Fridge("Ełk", "Ełk", "Ełk");
            fridgeElk.AddUser(user3);

            var fridge2 = new Fridge("Lodówa2", "Lodówa2", "Lodówa2");
            fridge2.AddUser(user4);

            var fridge3 = new Fridge("Lodówa3", "Lodówa3", "Lodówa3");
            fridge3.AddUser(user5);

            fridges.Add(fridgeDragana);
            fridges.Add(fridgeElk);
            fridges.Add(fridge2);
            fridges.Add(fridge3);

            return fridges;
        }

        private IEnumerable<Recipe> GenerateRecipes(IEnumerable<FoodProduct> list)
        {
            var recipes = new List<Recipe>();

            recipes.Add(KurczakSlodkoKwasny(list));
            recipes.Add(PiersKurczakaWPanierce(list));
            recipes.Add(LeczoZTofu(list));
            recipes.Add(Jajecznica(list));
            recipes.Add(Nalesniki(list));
            recipes.Add(NalesnikiBanan(list));
            recipes.Add(StekWolowy(list));

            return recipes;
        }

        private IEnumerable<FoodProduct> GenerateFoodProducts()
        {
            var categoryWarzywa = new Category("Warzywa");
            var categoryOwoce = new Category("Owoce");
            var categoryNabial = new Category("Nabiał i jaja");
            var categorySlodycze = new Category("Słodycze");
            var categoryMieso = new Category("Mięso");
            var categoryNapoje = new Category("Napoje");
            var categoryPieczywo = new Category("Pieczywo");
            var categoryMakarony = new Category("Makarony, kasze i ryże");
            var categoryRyby = new Category("Ryby i owoce morza");
            var categoryOrzechy = new Category("Orzechy i nasiona");
            var categoryTluszcze = new Category("Tłuszcze");
            var categoryInne = new Category("Inne");

            foodProducts.Add(new FoodProduct("Ciecierzyca", categoryInne));
            foodProducts.Add(new FoodProduct("Soczewica", categoryInne));
            foodProducts.Add(new FoodProduct("Musli", categoryInne));
            foodProducts.Add(new FoodProduct("Owsianka", categoryInne));
            foodProducts.Add(new FoodProduct("Keczup", categoryInne));
            foodProducts.Add(new FoodProduct("Musztarda", categoryInne));
            foodProducts.Add(new FoodProduct("Ocet", categoryInne));
            foodProducts.Add(new FoodProduct("Pieprz", categoryInne));
            foodProducts.Add(new FoodProduct("Sól", categoryInne));
            foodProducts.Add(new FoodProduct("Syrop klonowy", categoryInne));
            foodProducts.Add(new FoodProduct("Papryka słodka", categoryInne));
            foodProducts.Add(new FoodProduct("Papryka ostra", categoryInne));
            foodProducts.Add(new FoodProduct("Oregano", categoryInne));
            foodProducts.Add(new FoodProduct("Majeranek", categoryInne));
            foodProducts.Add(new FoodProduct("Imbir", categoryInne));
            foodProducts.Add(new FoodProduct("Ziele angielskie", categoryInne));
            foodProducts.Add(new FoodProduct("Zioła prowansalskie", categoryInne));
            foodProducts.Add(new FoodProduct("Sos słodko-kwaśny", categoryInne));
            foodProducts.Add(new FoodProduct("Bułka tarta", categoryInne));
            foodProducts.Add(new FoodProduct("Mąka", categoryInne));
            foodProducts.Add(new FoodProduct("Tofu", categoryInne));

            foodProducts.Add(new FoodProduct("Oliwa z oliwek", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Olej", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Masło", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Margaryna", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Olej kokosowy", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Masło klarowane", categoryTluszcze));
            foodProducts.Add(new FoodProduct("Smalec", categoryTluszcze));

            foodProducts.Add(new FoodProduct("Orzechy laskowe", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Orzechy włoskie", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Migdały", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Orzechy ziemne", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Orzechy nerkowca", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Orzechy pekan", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Orzechy pistacje", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Ziarna słonecznika", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Sezam", categoryOrzechy));
            foodProducts.Add(new FoodProduct("Pestki dynii", categoryOrzechy));

            foodProducts.Add(new FoodProduct("Łosoś", categoryRyby));
            foodProducts.Add(new FoodProduct("Makrela", categoryRyby));
            foodProducts.Add(new FoodProduct("Dorsz", categoryRyby));
            foodProducts.Add(new FoodProduct("Śledź", categoryRyby));
            foodProducts.Add(new FoodProduct("Palusz rybne", categoryRyby));
            foodProducts.Add(new FoodProduct("Miruna", categoryRyby));
            foodProducts.Add(new FoodProduct("Morszczuk", categoryRyby));
            foodProducts.Add(new FoodProduct("Halibut", categoryRyby));
            foodProducts.Add(new FoodProduct("Pstrąg", categoryRyby));
            foodProducts.Add(new FoodProduct("Tuńczyk", categoryRyby));
            foodProducts.Add(new FoodProduct("Krewetki", categoryRyby));
            foodProducts.Add(new FoodProduct("Paluszki krabowe", categoryRyby));

            foodProducts.Add(new FoodProduct("Makaron rurki (penne)", categoryMakarony));
            foodProducts.Add(new FoodProduct("Makaron spaghetti", categoryMakarony));
            foodProducts.Add(new FoodProduct("Ryż biały", categoryMakarony));
            foodProducts.Add(new FoodProduct("Ryż basmati", categoryMakarony));
            foodProducts.Add(new FoodProduct("Ryż jaśminowy", categoryMakarony));
            foodProducts.Add(new FoodProduct("Ryż do sushi", categoryMakarony));
            foodProducts.Add(new FoodProduct("Ryż brązowy", categoryMakarony));
            foodProducts.Add(new FoodProduct("Makaron do zup", categoryMakarony));
            foodProducts.Add(new FoodProduct("Makaron świderki", categoryMakarony));
            foodProducts.Add(new FoodProduct("Kasza kuskus", categoryMakarony));
            foodProducts.Add(new FoodProduct("Kasza gryczana", categoryMakarony));
            foodProducts.Add(new FoodProduct("Kasza pęczak", categoryMakarony));
            foodProducts.Add(new FoodProduct("Kasza jaglana", categoryMakarony));

            foodProducts.Add(new FoodProduct("Biały chleb", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Chleb razowy", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Bułki", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Chleb żytni", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Ciabatta", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Bagietka", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Bagietka czosnkowa", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Chleb tostowy", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Wafle kukurydziane", categoryPieczywo));
            foodProducts.Add(new FoodProduct("Wafle", categoryPieczywo));

            foodProducts.Add(new FoodProduct("Woda", categoryNapoje));
            foodProducts.Add(new FoodProduct("Piwo", categoryNapoje));
            foodProducts.Add(new FoodProduct("Whisky", categoryNapoje));
            foodProducts.Add(new FoodProduct("Wódka", categoryNapoje));
            foodProducts.Add(new FoodProduct("Sok ", categoryNapoje));
            foodProducts.Add(new FoodProduct("Syrop", categoryNapoje));
            foodProducts.Add(new FoodProduct("Wino", categoryNapoje));
            foodProducts.Add(new FoodProduct("Napoje gazowane", categoryNapoje));
            foodProducts.Add(new FoodProduct("Herbata", categoryNapoje));
            foodProducts.Add(new FoodProduct("Kawa", categoryNapoje));

            foodProducts.Add(new FoodProduct("Czekolada", categorySlodycze));
            foodProducts.Add(new FoodProduct("Baton", categorySlodycze));
            foodProducts.Add(new FoodProduct("Gofry", categorySlodycze));
            foodProducts.Add(new FoodProduct("Lody", categorySlodycze));
            foodProducts.Add(new FoodProduct("Czekolada", categorySlodycze));
            foodProducts.Add(new FoodProduct("Chipsy", categorySlodycze));
            foodProducts.Add(new FoodProduct("Chrupki", categorySlodycze));
            foodProducts.Add(new FoodProduct("Masło orzechowe", categorySlodycze));
            foodProducts.Add(new FoodProduct("Miód", categorySlodycze));
            foodProducts.Add(new FoodProduct("Cukier", categorySlodycze));
            foodProducts.Add(new FoodProduct("Cukier puder", categorySlodycze));

            foodProducts.Add(new FoodProduct("Banan", categoryOwoce));
            foodProducts.Add(new FoodProduct("Jabłko", categoryOwoce));
            foodProducts.Add(new FoodProduct("Gruszka", categoryOwoce));
            foodProducts.Add(new FoodProduct("Kiwi", categoryOwoce));
            foodProducts.Add(new FoodProduct("Maliny", categoryOwoce));
            foodProducts.Add(new FoodProduct("Cytryna", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Truskawki", categoryOwoce));
            foodProducts.Add(new FoodProduct("Rodzynki", categoryOwoce));
            foodProducts.Add(new FoodProduct("Pomarańcze", categoryOwoce));
            foodProducts.Add(new FoodProduct("Mandarynki", categoryOwoce));
            foodProducts.Add(new FoodProduct("Śliwki", categoryOwoce));
            foodProducts.Add(new FoodProduct("Wiśnie", categoryOwoce));
            foodProducts.Add(new FoodProduct("Żurawina", categoryOwoce));
            foodProducts.Add(new FoodProduct("Daktyle", categoryOwoce));
            foodProducts.Add(new FoodProduct("Morele", categoryOwoce));
            foodProducts.Add(new FoodProduct("Mango", categoryOwoce));

            foodProducts.Add(new FoodProduct("Bakłażan", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Brokuł", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Burak", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Cebula", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Cukinia", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Czosnek", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Dynia", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Groszek", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Jarmuż", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Grzyby", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Kalafior", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Kapusta", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Kukurydza", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Marchewka", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Ogórek", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Ogórek kiszony", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Oliwki", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Papryka", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Marchew", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Pomidor", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Rzodkiewka", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Sałata", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Szpinak", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Szparagi", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Ziemniaki", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Por", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Pietruszka", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Roszponka", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Rukola", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Koperek", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Natka pietruszki", categoryWarzywa));
            foodProducts.Add(new FoodProduct("Bazylia", categoryWarzywa));

            foodProducts.Add(new FoodProduct("Jogurt naturalny", categoryNabial));
            foodProducts.Add(new FoodProduct("Śmietana 18", categoryNabial));
            foodProducts.Add(new FoodProduct("Śmietana 30", categoryNabial));
            foodProducts.Add(new FoodProduct("Twaróg", categoryNabial));
            foodProducts.Add(new FoodProduct("Ser żółty", categoryNabial));
            foodProducts.Add(new FoodProduct("Parmezan", categoryNabial));
            foodProducts.Add(new FoodProduct("Camembert", categoryNabial));
            foodProducts.Add(new FoodProduct("Ser kozi", categoryNabial));
            foodProducts.Add(new FoodProduct("Ser pleśniowy", categoryNabial));
            foodProducts.Add(new FoodProduct("Mleko", categoryNabial));
            foodProducts.Add(new FoodProduct("Cheddar", categoryNabial));
            foodProducts.Add(new FoodProduct("Mozzarella", categoryNabial));
            foodProducts.Add(new FoodProduct("Jaja", categoryNabial));

            foodProducts.Add(new FoodProduct("Boczek", categoryMieso));
            foodProducts.Add(new FoodProduct("Cielęcina", categoryMieso));
            foodProducts.Add(new FoodProduct("Indyk", categoryMieso));
            foodProducts.Add(new FoodProduct("Pierś z kurczaka", categoryMieso));
            foodProducts.Add(new FoodProduct("Kiełbasa", categoryMieso));
            foodProducts.Add(new FoodProduct("Udka z kurczaka", categoryMieso));
            foodProducts.Add(new FoodProduct("Skrzydełka z kurczaka", categoryMieso));
            foodProducts.Add(new FoodProduct("Nogi z kurczaka", categoryMieso));
            foodProducts.Add(new FoodProduct("Schab", categoryMieso));
            foodProducts.Add(new FoodProduct("Mięso mielone wołowe", categoryMieso));
            foodProducts.Add(new FoodProduct("Mięso mielone wieprzowe", categoryMieso));
            foodProducts.Add(new FoodProduct("Karkówka", categoryMieso));
            foodProducts.Add(new FoodProduct("Żeberka", categoryMieso));
            foodProducts.Add(new FoodProduct("Mięso wołowe", categoryMieso));
            foodProducts.Add(new FoodProduct("Tatar z wołowiny", categoryMieso));
            foodProducts.Add(new FoodProduct("Parówki", categoryMieso));
            foodProducts.Add(new FoodProduct("Wędlina", categoryMieso));
            foodProducts.Add(new FoodProduct("Stek wołowy", categoryMieso));

            return foodProducts;
        }


        private Recipe KurczakSlodkoKwasny(IEnumerable<FoodProduct> list)
        {

            var kurczak = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Pierś z kurczaka")).SingleOrDefault(),
                new AmountValue(250, Unit.Grams));
            var ryz = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Ryż biały")).SingleOrDefault(),
                new AmountValue(100, Unit.Grams));
            var cebula = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Cebula")).SingleOrDefault(),
                new AmountValue(1, Unit.Pieces));
            var sos_slodko_kwasny = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Sos słodko-kwaśny")).SingleOrDefault(),
               new AmountValue(200, Unit.Grams));

            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(kurczak);
            _foodProducts.Add(ryz);
            _foodProducts.Add(cebula);
            _foodProducts.Add(sos_slodko_kwasny);

            var desc = "Pierś z kurczaka pokroić w kostkę, przyprawić i wstawić na 30 min do lodówki. Ryż zagotować. Obrac cebulę, pokroić w kostkę. Pokroić czosnek (lub dodać czosnek granulowany). Razem z cebulą podsmażyć na oleju/maśle przez minutę. Dodać kawałki kurczaka i smażyć razem 5-6 minut. Dodać sos słodok-kwaśny i całość dusić jeszcze przez 10 min. Podawać razem z ryżem.";

            var recipe = new Recipe("Kurczak w sosie słodko-kwaśnym", desc, categoryObiad, _foodProducts, 30, (int)LevelOfDifficulty.Easy);

            return recipe;
        }

        private Recipe PiersKurczakaWPanierce(IEnumerable<FoodProduct> list)
        {
            var kurczak = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Pierś z kurczaka")).SingleOrDefault(),
                new AmountValue(250, Unit.Grams));
            var ziemniaki = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Ziemniaki")).SingleOrDefault(),
                new AmountValue(300, Unit.Grams));
            var bulka_tarta = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Bułka tarta")).SingleOrDefault(),
               new AmountValue(50, Unit.Grams));
            var maka = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(),
               new AmountValue(50, Unit.Grams));
            var jajo = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(),
               new AmountValue(1, Unit.Pieces));


            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(kurczak);
            _foodProducts.Add(ziemniaki);
            _foodProducts.Add(bulka_tarta);
            _foodProducts.Add(maka);
            _foodProducts.Add(jajo);

            var desc = "Pierś z kurczaka pokroic na kotlety (można delikatnie rozbić młotkiem), przyprawić i wstawić na 30 min do lodówki. Ziemniaki wstawić do gotowania. Przygotować mąke, bułkę tartą i jajka w naczyniach. Na patelni rozgrzać olej i smażyć kotlety (umoczone najpierw w mące, potem w jaju i na koniec w bułce tartej). Podawać z surówka/mizerią.";

            var recipe = new Recipe("Kotlety schabowe z piersi kurczaka", desc, categoryObiad, _foodProducts, 40, (int)LevelOfDifficulty.Easy);

            return recipe;
        }

        private Recipe LeczoZTofu(IEnumerable<FoodProduct> list)
        {
            var tofu = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Tofu")).SingleOrDefault(),
                new AmountValue(300, Unit.Grams));
            var cukinia = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Cukinia")).SingleOrDefault(),
                new AmountValue(1, Unit.Pieces));
            var papryka = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Papryka")).SingleOrDefault(),
               new AmountValue(1, Unit.Pieces));
            var cebula = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Cebula")).SingleOrDefault(),
               new AmountValue(1, Unit.Pieces));
            var czosnek = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Czosnek")).SingleOrDefault(),
               new AmountValue(2, Unit.Pieces));
            var pomidory = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Pomidor")).SingleOrDefault(),
               new AmountValue(3, Unit.Pieces));

            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(tofu);
            _foodProducts.Add(cukinia);
            _foodProducts.Add(papryka);
            _foodProducts.Add(cebula);
            _foodProducts.Add(czosnek);
            _foodProducts.Add(pomidory);

            var desc = "Tofu pokroić w kostkę, przyprawić i zostawić na trochę w lodówce. Cukinię pokroić w półtalarki, paprykę w kostkę, pomidora na małe kawałki (żeby puścił sok). Cebulę i czonske kroimy i podsmażamy na oliwie w garnku. Po minucie dodajemy cukinie, dodajemy 100ml wody, dusimy przez 3 min. Dodajemy pomidory i dalej dusimy. Podsmażamy tofu na patelni - w międzyczasie dorzucamy paprykę do lecza (w zależności jak chcemy żeby była twarda). Jak tofu jest podsmażone, miezsamy wszystko w garnku.";

            var recipe = new Recipe("Leczo z tofu, cukinii i papryki", desc, categoryObiad, _foodProducts, 35, (int)LevelOfDifficulty.Easy);

            return recipe;
        }

        private Recipe Jajecznica(IEnumerable<FoodProduct> list)
        {
            var jaja = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(),
                new AmountValue(3, Unit.Pieces));
            var mleko = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(),
               new AmountValue(75, Unit.Mililiter));
            mleko.SetOptional();
            var cebula = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Cebula")).SingleOrDefault(),
               new AmountValue(50, Unit.Grams));
            cebula.SetOptional();
            var maslo = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Masło")).SingleOrDefault(),
               new AmountValue(15, Unit.Grams));

            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(jaja);
            _foodProducts.Add(mleko);
            _foodProducts.Add(maslo);
            _foodProducts.Add(cebula);

            var desc = "Rozbijamy jajka, dodajemy trochę mleka. Na patelni rozgrzewamy masło i wrzucamy cebulę. Po chwili wlewamy jajka i czekamy aż się zetną.";

            var recipe = new Recipe("Jajecznica na mleku", desc, categorySniadanie, _foodProducts, 10, (int)LevelOfDifficulty.Easy);

            return recipe;
        }

        private Recipe Nalesniki(IEnumerable<FoodProduct> list)
        {
            var jaja = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(),
                new AmountValue(2, Unit.Pieces));
            var maka = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(),
                new AmountValue(200, Unit.Grams));
            var mleko = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(),
               new AmountValue(150, Unit.Mililiter));
            var woda = new FoodProductDetails(
              list.Where(x => x.Name.Equals("Woda")).SingleOrDefault(),
              new AmountValue(30, Unit.Mililiter));
            woda.SetOptional();
            var olej = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Olej")).SingleOrDefault(),
             new AmountValue(15, Unit.Mililiter));


            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(jaja);
            _foodProducts.Add(mleko);
            _foodProducts.Add(maka);
            _foodProducts.Add(woda);

            var desc = "Do mąki wbijamy jajko i mleko, mieszamy. Dodajemy troche wody gazowanej. Dodajemy trochę soli. Wlewamy na patelnie do naleśników (mozna wytrzec olejem przed smazeniem) i smażymy.";

            var recipe = new Recipe("Naleśniki", desc, categoryKolacja, _foodProducts, 25, (int)LevelOfDifficulty.Easy);

            return recipe;
        }

        private Recipe NalesnikiBanan(IEnumerable<FoodProduct> list)
        {
            var jaja = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Jaja")).SingleOrDefault(),
                new AmountValue(2, Unit.Pieces));
            var maka = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Mąka")).SingleOrDefault(),
                new AmountValue(80, Unit.Grams));
            var mleko = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Mleko")).SingleOrDefault(),
               new AmountValue(150, Unit.Mililiter));
            var maslo = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Masło")).SingleOrDefault(),
               new AmountValue(30, Unit.Grams));
            var olej = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Olej")).SingleOrDefault(),
             new AmountValue(20, Unit.Mililiter));
            var banan = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Banan")).SingleOrDefault(),
             new AmountValue(6, Unit.Pieces));
            var sol = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Sól")).SingleOrDefault(),
             new AmountValue(10, Unit.Grams));


            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(jaja);
            _foodProducts.Add(mleko);
            _foodProducts.Add(maka);
            _foodProducts.Add(maslo);
            _foodProducts.Add(olej);
            _foodProducts.Add(sol);
            _foodProducts.Add(banan);

            var desc = "Ciasto: roztopił 1 łyżkę masła, wszystkie składniki wymieszać, dodając taką ilość mleka, aby ciasto miało konsystencję gęstej śmietany. Odstawić na godzinę. Smażyć naleśniki na patelni posmarowanej olejem. Nadzienie: banany obrać, obsmażyć na reszcie masła na małym ogniu tak, aby go nie rumienić. Gdy banany lekko zmiękną, posypać grubo cukrem pudrem i skropić aromatem rumowym. Od czasu do czasu skrapiać wodą, do momentu, aż na wierzchu powstanie karmel.Lekko przestudzone banany owinąć naleśnikiem, ułożyć w żaroodpornym naczyniu do zapiekania. Posypać cukrem i ponownie skropić aromatem rumowym";

            var recipe = new Recipe("Naleśniki z bananami", desc, categoryKolacja, _foodProducts, 90, (int)LevelOfDifficulty.Medium);

            return recipe;
        }

        private Recipe StekWolowy(IEnumerable<FoodProduct> list)
        {
            var stek = new FoodProductDetails(
                list.Where(x => x.Name.Equals("Stek wołowy")).SingleOrDefault(),
                new AmountValue(3, Unit.Pieces));
            var maslo = new FoodProductDetails(
               list.Where(x => x.Name.Equals("Masło")).SingleOrDefault(),
               new AmountValue(60, Unit.Grams));
            var sol = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Sól")).SingleOrDefault(),
             new AmountValue(10, Unit.Grams));
            var pieprz = new FoodProductDetails(
             list.Where(x => x.Name.Equals("Pieprz")).SingleOrDefault(),
             new AmountValue(10, Unit.Grams));


            var _foodProducts = new List<FoodProductDetails>();
            _foodProducts.Add(stek);
            _foodProducts.Add(maslo);
            _foodProducts.Add(sol);
            _foodProducts.Add(pieprz);

            var desc = "3-4 minut na stek mocno krwisty. 4-5 minut na stek krwisty. 5-6 minut na stek średnio wysmażony. 6-7 minut na stek mocno wysmażony";

            var recipe = new Recipe("Stek wołowy z patelni", desc, categoryObiad, _foodProducts, 20, (int)LevelOfDifficulty.Hard);

            return recipe;
        }
    }
}
