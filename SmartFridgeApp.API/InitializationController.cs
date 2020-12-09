using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using SmartFridgeApp.API.Fridges.GetFridges;
using SmartFridgeApp.Domain.Models.FoodProducts;
using SmartFridgeApp.Domain.SeedWork;
using System.Threading;
using System.Collections.Generic;
using SmartFridgeApp.Infrastructure;

namespace SmartFridgeApp.API
{
    [Route("api/init")]
    [ApiController]
    public class InitializationController : Controller
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly SmartFridgeAppContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public InitializationController(
            IUnitOfWork unitOfWork, IFoodProductRepository foodProductRepository, SmartFridgeAppContext context)
        {
            _unitOfWork = unitOfWork;
            _foodProductRepository = foodProductRepository;
            _context = context;
        }

        /// <summary>
        /// Init Database
        /// </summary>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IActionResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> InitDatabase()
        {
            var foodProducts = GenerateFoodProducts();
            foreach (var fp in foodProducts)
            {
                await _context.FoodProducts.AddAsync(fp);
            }
           
            await _context.SaveChangesAsync();

            return Ok();
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

            List<FoodProduct> foodProducts = new List<FoodProduct>();

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
            foodProducts.Add(new FoodProduct("Marchewka", categoryWarzywa));
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

    }
}
