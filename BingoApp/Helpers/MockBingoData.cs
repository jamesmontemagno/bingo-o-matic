using BingoApp.Models;

namespace BingoApp.Helpers;

public static class MockBingoData
{
    public static List<BingoSet> GetMockSets() => new()
    {
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Popular Dog Breeds",
            Items = new[]
            {
                "Labrador Retriever", "French Bulldog", "Golden Retriever", "German Shepherd", "Poodle",
                "Bulldog", "Rottweiler", "Beagle", "Dachshund", "Yorkshire Terrier",
                "Boxer", "Great Dane", "Siberian Husky", "Doberman Pinscher", "Australian Shepherd",
                "Miniature Schnauzer", "Cavalier King Charles Spaniel", "Shih Tzu", "Boston Terrier", "Pomeranian",
                "Havanese", "Shetland Sheepdog", "Bernese Mountain Dog", "Brittany", "Cocker Spaniel",
                "Border Collie", "Pembroke Welsh Corgi", "Chihuahua", "Pug", "Mastiff",
                "English Springer Spaniel", "Belgian Malinois", "Vizsla", "Basset Hound", "Portuguese Water Dog",
                "Maltese", "Newfoundland", "West Highland White Terrier", "Bichon Frise", "Rhodesian Ridgeback",
                "Chesapeake Bay Retriever", "Collie", "St. Bernard", "Bull Terrier", "Scottish Terrier",
                "Bloodhound", "Akita", "Alaskan Malamute", "Chow Chow", "Old English Sheepdog",
                "Irish Setter", "Afghan Hound", "Basenji", "Samoyed",
                "Great Pyrenees", "Weimaraner", "Irish Wolfhound", "Airedale Terrier", "Bullmastiff",
                "Dalmatian", "Papillon", "Giant Schnauzer", "Australian Cattle Dog",
                "Soft Coated Wheaten Terrier", "Cairn Terrier", "Chinese Shar-Pei", "Norwegian Elkhound", "Whippet",
                "Italian Greyhound", "Border Terrier", "Wirehaired Pointing Griffon", "Keeshond", "Belgian Tervuren",
                "Cardigan Welsh Corgi", "Greater Swiss Mountain Dog", "Chinese Crested", "Staffordshire Bull Terrier"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Dog Behaviors & Activities",
            Items = new[]
            {
                "Wagging Tail", "Fetch", "Belly Rubs", "Chasing Squirrels", "Barking at Mail Carrier",
                "Digging Holes", "Chewing Bones", "Playing Tug-of-War", "Rolling in Grass", "Chasing Tennis Balls",
                "Begging for Food", "Sleeping in Sun", "Head Tilts", "Zoomies", "Catching Treats",
                "Sniffing Everything", "Licking Faces", "Herding", "Swimming", "Pawing for Attention",
                "Howling", "Chasing Own Tail", "Guard Dog Mode", "Shaking Toys", "Puppy Eyes",
                "Stealing Socks", "Jumping for Joy", "Snoring", "Following Owner", "Leash Walking",
                "Agility Training", "Dog Park Play", "Car Rides", "Beach Running", "Nose Work",
                "Trick Training", "Cuddling", "Hiding Toys", "Dock Diving", "Tracking Scents",
                "Frisbee Catching", "Hiking", "Camping", "Snow Playing", "Puddle Splashing",
                "Treat Puzzles", "Ball Obsession", "Stick Collecting", "Leaf Chasing", "Bird Watching",
                "Door Greeting", "Couch Napping", "Kong Time", "Grooming", "Training Sessions",
                "Flyball", "Rally Obedience", "Therapy Work", "Search and Rescue", "Sheep Herding",
                "Dock Jumping", "Lure Coursing", "Scent Work", "Barn Hunt", "Show Ring",
                "Carting", "Weight Pull", "Musical Freestyle", "Earthdog Trials", "Disc Dog",
                "Jogging Partner", "Water Rescue", "Protection Work", "Skateboarding", "Surfing"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Birds of North America",
            Items = new[]
            {
                "American Robin", "Northern Cardinal", "Blue Jay", "Black-capped Chickadee", "American Goldfinch",
                "House Finch", "Downy Woodpecker", "White-breasted Nuthatch", "American Crow", "Mourning Dove",
                "European Starling", "House Sparrow", "Red-winged Blackbird", "Northern Mockingbird", "Eastern Bluebird",
                "Tufted Titmouse", "Purple Finch", "Dark-eyed Junco", "Song Sparrow", "Carolina Wren",
                "Red-bellied Woodpecker", "Common Grackle", "American Eagle", "Red-tailed Hawk", "Great Blue Heron",
                "Barn Swallow", "Eastern Phoebe", "Ruby-throated Hummingbird", "Northern Flicker", "Cooper's Hawk",
                "Mallard Duck", "Canada Goose", "Belted Kingfisher", "Pileated Woodpecker", "Cedar Waxwing",
                "Evening Grosbeak", "Baltimore Oriole", "Rose-breasted Grosbeak", "Pine Siskin", "American Kestrel",
                "Barn Owl", "Great Horned Owl", "Screech Owl", "Turkey Vulture", "Osprey",
                "Wood Duck", "Sandhill Crane", "Wild Turkey", "Ruffed Grouse", "Common Loon",
                "Great Egret", "Snowy Owl", "Red-shouldered Hawk", "Ring-billed Gull", "Black-crowned Night Heron",
                "Eastern Towhee", "Indigo Bunting", "Northern Harrier", "Sharp-shinned Hawk", "American Woodcock",
                "Yellow Warbler", "Scarlet Tanager", "Wood Thrush", "Purple Martin",
                "Tree Swallow", "Gray Catbird", "Brown Thrasher", "Common Yellowthroat", "Eastern Meadowlark",
                "Peregrine Falcon", "Common Raven", "Red Crossbill", "Yellow-bellied Sapsucker", "Pine Grosbeak"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Central Oregon Attractions",
            Items = new[]
            {
                "Smith Rock", "Mt. Bachelor", "Tumalo Falls", "Deschutes River", "Pilot Butte",
                "Old Mill District", "Drake Park", "Lava Butte", "Newberry Volcano", "Cascade Lakes",
                "High Desert Museum", "Les Schwab Amphitheater", "Bend Whitewater Park", "Mirror Pond", "Phil's Trail",
                "Shevlin Park", "Deschutes Brewery", "Crux Fermentation Project", "Bend Factory Outlets", "Worthy Brewing",
                "Sunriver Resort", "Eagle Crest Resort", "Black Butte Ranch", "Todd Lake", "Sparks Lake",
                "Elk Lake", "Cultus Lake", "Paulina Peak", "Lava River Cave", "Boyd Cave",
                "Arnold Ice Cave", "Skylight Cave", "Paulina Lake", "East Lake", "Broken Top",
                "South Sister", "Green Lakes", "Moraine Lake", "Devils Lake", "Hosmer Lake",
                "Proxy Falls", "Sahalie Falls", "Koosah Falls", "Benham Falls", "Dillon Falls",
                "Big Eddy", "Farewell Bend Park", "Riverbend Park", "McKay Park", "Pioneer Park",
                "Hollinshead Park", "Pine Nursery Park", "Discovery Park", "Rock Ridge Park", "Hillside Park",
                "Summit Park", "Wildflower Park", "Sawyer Park", "First Street Rapids", "Columbia Park",
                "Juniper Park", "Harmon Park", "Brooks Park", "Compass Park", "Pacific Crest Trail",
                "Deschutes River Trail", "Metolius River", "Suttle Lake", "Blue Pool", "Camp Abbot Trading Co.",
                "Lava Lands Visitor Center", "Tower Theatre", "Cascade Village", "Old Mill Marketplace", "The Box Factory"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Famous Mountains",
            Items = new[]
            {
                "Mount Everest", "K2", "Matterhorn", "Mount Kilimanjaro", "Mount Fuji",
                "Denali", "Mount Hood", "Mount Rainier", "Mont Blanc", "Mount Whitney",
                "Mount St. Helens", "Aconcagua", "Mount Elbrus", "Mount McKinley", "Kangchenjunga",
                "Lhotse", "Makalu", "Cho Oyu", "Dhaulagiri", "Manaslu",
                "Nanga Parbat", "Annapurna", "Gasherbrum I", "Broad Peak", "Gasherbrum II",
                "Shishapangma", "Mount Logan", "Mount Baker", "Mount Adams", "Three Sisters",
                "Mount Shasta", "Mount Bachelor", "Broken Top", "Mount Jefferson", "Mount Washington",
                "Pikes Peak", "Mount Mitchell", "Grand Teton", "Mount Olympus", "Mount Thielsen",
                "Black Butte", "Mount McLoughlin", "Mount Scott", "Diamond Peak", "Mount Bailey",
                "South Sister", "Middle Sister", "North Sister", "Mount Yoran",
                "Mount Hood Meadows", "Timberline Lodge", "Three Fingered Jack",
                "Lassen Peak", "Mount Elbert", "Mount Massive",
                "Mount Harvard", "Mount Princeton", "Mount Yale", "Mount Oxford", "Mount Columbia"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "PNW Geographic Features",
            Items = new[]
            {
                "Puget Sound", "Columbia River", "Willamette Valley", "Olympic Peninsula", "San Juan Islands",
                "Mount Rainier", "Mount Hood", "Mount St. Helens", "Mount Baker", "North Cascades",
                "Olympic Mountains", "Coast Range", "Cascade Range", "Columbia River Gorge", "Hoh Rainforest",
                "Oregon Coast", "Washington Coast", "Olympic National Park", "Crater Lake", "Smith Rock",
                "Deschutes River", "Snake River", "John Day River", "Willamette River", "Hood River",
                "Mount Adams", "Mount Jefferson", "Three Sisters", "Broken Top",
                "Mount Bachelor", "Newberry Volcano", "Steens Mountain", "Wallowa Mountains", "Blue Mountains",
                "Painted Hills", "Alvord Desert", "Fort Rock", "Cascade Lakes",
                "Elliott Bay", "Lake Washington", "Lake Chelan", "Ross Lake", "Lake Oswego",
                "Klamath Lake", "Lake Billy Chinook", "Detroit Lake", "Clear Lake",
                "Olympic Rain Shadow", "Columbia Basin", "Palouse Hills", "Methow Valley", "Skagit Valley",
                "Rogue Valley", "Umpqua Valley", "Hood River Valley", "Yakima Valley",
                "Olympic Coast", "Long Beach Peninsula", "Tillamook Head", "Cape Disappointment",
                "Cape Lookout", "Cape Kiwanda", "Haystack Rock", "Three Arch Rocks", "Thor's Well"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "World Capital Cities",
            Items = new[]
            {
                "Tokyo", "Beijing", "Moscow", "London", "Paris",
                "Berlin", "Rome", "Madrid", "Lisbon", "Amsterdam",
                "Brussels", "Vienna", "Copenhagen", "Stockholm", "Oslo",
                "Helsinki", "Tallinn", "Riga", "Vilnius", "Warsaw",
                "Prague", "Budapest", "Bratislava", "Ljubljana", "Zagreb",
                "Sarajevo", "Belgrade", "Skopje", "Tirana", "Athens",
                "Bucharest", "Sofia", "Chisinau", "Kiev", "Minsk",
                "Dublin", "Reykjavik", "Ottawa", "Washington DC", "Mexico City",
                "Bogota", "Lima", "Santiago", "Buenos Aires", "Brasilia",
                "Caracas", "Quito", "La Paz", "Asuncion", "Montevideo",
                "Cairo", "Addis Ababa", "Nairobi", "Pretoria", "Cape Town",
                "Rabat", "Algiers", "Tunis", "Tripoli", "Dakar",
                "Bangkok", "Hanoi", "Manila", "Jakarta", "Kuala Lumpur",
                "Singapore", "Seoul", "Ulaanbaatar", "New Delhi", "Islamabad",
                "Tehran", "Baghdad", "Riyadh", "Damascus", "Jerusalem",
                "Ankara", "Baku", "Tbilisi", "Yerevan", "Astana"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "North American Wildlife",
            Items = new[]
            {
                "Grizzly Bear", "Black Bear", "Wolf", "Mountain Lion", "Moose",
                "Elk", "Bison", "Bighorn Sheep", "Mountain Goat", "White-tailed Deer",
                "Mule Deer", "Caribou", "Pronghorn", "Beaver", "River Otter",
                "Sea Otter", "Harbor Seal", "Sea Lion", "Walrus", "Orca",
                "Gray Whale", "Humpback Whale", "Dolphin", "Bald Eagle", "Golden Eagle",
                "Osprey", "Red-tailed Hawk", "Great Horned Owl", "Snowy Owl", "Wild Turkey",
                "Canada Goose", "Trumpeter Swan", "Sandhill Crane", "Great Blue Heron", "American Alligator",
                "Bobcat", "Lynx", "Fox", "Coyote", "Raccoon",
                "Opossum", "Skunk", "Porcupine", "Wolverine", "Fisher",
                "Marten", "Mink", "Weasel", "Badger", "Gray Wolf",
                "Red Fox", "Arctic Fox", "Snowshoe Hare", "Jack Rabbit", "Cotton-tail Rabbit",
                "Ground Squirrel", "Chipmunk", "Prairie Dog", "Marmot", "Flying Squirrel",
                "Red Squirrel", "Gray Squirrel", "Pika", "Muskrat", "Vole",
                "Jaguar", "Ocelot", "Armadillo", "Javelina",
                "Dall Sheep"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Classic TV Shows",
            Items = new[]
            {
                "I Love Lucy", "The Twilight Zone", "M*A*S*H", "Cheers", "Friends",
                "Seinfeld", "The Office", "Breaking Bad", "Game of Thrones", "The Simpsons",
                "Star Trek", "Doctor Who", "The Brady Bunch", "Happy Days", "All in the Family",
                "The Mary Tyler Moore Show", "Miami Vice", "MacGyver", "The A-Team", "Knight Rider",
                "Magnum P.I.", "Dallas", "Dynasty", "The Golden Girls", "Family Ties",
                "The Cosby Show", "Growing Pains", "Full House", "Fresh Prince of Bel-Air", "Saved by the Bell",
                "The X-Files", "ER", "Law & Order", "NYPD Blue", "Buffy the Vampire Slayer",
                "The West Wing", "The Sopranos", "Sex and the City", "Lost", "24",
                "Grey's Anatomy", "House", "CSI", "The Wire", "Mad Men",
                "Dexter", "The Big Bang Theory", "Modern Family", "How I Met Your Mother", "Parks and Recreation",
                "Stranger Things", "The Crown", "This Is Us", "The Mandalorian", "The Walking Dead",
                "Westworld", "Black Mirror", "The Good Place", "Schitt's Creek", "The Handmaid's Tale",
                "Frasier", "Will & Grace", "Everybody Loves Raymond", "The Practice", "Six Feet Under", "The Shield", "Deadwood"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Colorado Landmarks",
            Items = new[]
            {
                "Garden of the Gods", "Red Rocks Amphitheatre", "Pikes Peak", "Royal Gorge Bridge", "Mesa Verde National Park",
                "Great Sand Dunes National Park", "Maroon Bells", "Flatirons", "Chapel on the Rock", "Coors Field",
                "Union Station", "Denver Art Museum", "Colorado State Capitol", "Stanley Hotel", "Durango & Silverton Railroad",
                "Manitou Incline", "Mount Evans Scenic Byway", "Cheyenne Mountain Zoo", "Glenwood Hot Springs", "Telluride Historic District",
                "Leadville Mining District", "Bent's Old Fort", "Buffalo Bill Museum & Grave", "Crested Butte", "Pearl Street Mall",
                "Strawberry Park Hot Springs", "Royal Gorge Route Railroad", "San Juan Skyway", "Bishop Castle", "Hanging Lake",
                "Rocky Mountain National Park", "Denver Botanic Gardens", "Colorado Railroad Museum", "Chautauqua Park", "Elitch Gardens",
                "Molly Brown House Museum", "Mount Elbert", "Mount Sneffels", "Independence Pass", "Paint Mines Interpretive Park",
                "St. Mary's Glacier", "Seven Falls", "Loveland Pass", "Guanella Pass", "Cave of the Winds",
                "Echo Lake Park", "Crystal Mill", "Devil's Head Lookout", "Dinosaur National Monument", "Four Corners Monument"
            }
        },
        new BingoSet
        {
            Id = Guid.NewGuid(),
            Name = "Colorado Geography",
            Items = new[]
            {
                "Rocky Mountains", "Front Range", "San Juan Mountains", "Sawatch Range", "Sangre de Cristo Mountains",
                "Great Plains", "Colorado Plateau", "Arkansas River", "South Platte River", "Colorado River",
                "Gunnison River", "Yampa River", "San Luis Valley", "San Juan Basin", "Pawnee National Grassland",
                "Uncompahgre Plateau", "Grand Mesa", "Flat Tops", "Wet Mountain Valley", "Cache la Poudre River",
                "Blue River", "Roaring Fork Valley", "North Park", "Middle Park", "South Park",
                "Denver Basin", "Laramie Foothills", "Book Cliffs", "Dolores River", "Rabbit Ears Range",
                "Mount Elbert", "Mount Massive", "Mount Harvard", "Longs Peak", "Mount Antero",
                "Mount Lincoln", "Mount Wilson", "Mount Princeton", "Mount Yale", "Mount Bross",
                "Mount Democrat", "Mount Sherman", "Mount Shavano", "Mount Belford", "Mount Oxford",
                "Mount Columbia", "Mount Blanca", "Mount Lindsey", "Mount Ouray"
            }
        },
    };
}
