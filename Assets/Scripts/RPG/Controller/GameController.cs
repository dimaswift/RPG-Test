using RPG.Model;
using RPG.View;

namespace RPG.Controller
{
    public class GameController
    {
        public PlayerProfile PlayerProfile { get; private set; }
        
        readonly GameConfig _config;

        readonly UnitFactory _unitFactory;

        readonly IProfileProvider _profileProvider;

        readonly IRandomRange _randomRange;

        BattleController _battleController;
        
        public GameController(GameConfig config, IProfileProvider profileProvider, IRandomRange randomRange)
        {
            _config = config;
            _randomRange = randomRange;
            _profileProvider = profileProvider;
            _unitFactory = new UnitFactory(randomRange, _config.UnitVisualsAmount);
            PlayerProfile = profileProvider.LoadProfile();
            CheckHeroDeck();
            SaveProfile();
        }

        public void SaveProfile()
        {
            _profileProvider.SaveProfile(PlayerProfile);
           
        }
        
        public void StartBattle(IBattleView battleView)
        {
            SaveProfile();
            _battleController = new BattleController(_config, 
                PlayerProfile.Deck, 
                _unitFactory, 
                PlayerProfile.BattlesPlayed,
                _randomRange);
            _battleController.SetView(battleView);
            _battleController.PrepareBattle();
            _battleController.OnDefeat += ProcessBattleEnd;
            _battleController.OnVictory += ProcessBattleEnd;
        }

        void ProcessBattleEnd()
        {
            PlayerProfile.BattlesPlayed++;
            if (PlayerProfile.BattlesPlayed % _config.FreeHeroPrizeFrequency == 0
                && PlayerProfile.Deck.Count < _config.MaxHeroesCollectionSize)
            {
                var newHeroData = _unitFactory.CreateRandomHeroData(_config.HeroGeneratorConfig);
                PlayerProfile.Deck.Add(newHeroData);
            }
        
            _battleController.OnDefeat -= ProcessBattleEnd;
            _battleController.OnVictory -= ProcessBattleEnd;
            
            SaveProfile();
        }

        void CheckHeroDeck()
        {
            if (PlayerProfile.Deck.Count == 0)
            {
                PlayerProfile.Deck = _unitFactory.CreateHeroDeck(_config.InitialDeckSize, _config.HeroGeneratorConfig);
            }
        }
    
      
    }    
}