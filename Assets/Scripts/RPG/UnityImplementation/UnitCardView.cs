using RPG.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UnityImplementation
{
    [RequireComponent(typeof(HeroInfoTrigger))]
    public class UnitCardView : View, IPointerClickHandler
    {
        public HeroData HeroData
        {
            get { return _data; }
        }
        
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value; _selectionFrame.SetActive(value);
                if(_data != null)
                    _data.Selected = value;

            }
        }

        [SerializeField] Image _unitBodyImage;
        [SerializeField] Image _emptySlotImage;
        [SerializeField] GameObject _selectionFrame;

        HeroInfoTrigger _infoTrigger;
        HeroData _data;
        bool _selected;
        HeroCollectionView _collectionView;
        bool _isEmpty;


        protected override void OnInit(Game game)
        {
            base.OnInit(game);
            _infoTrigger = GetComponent<HeroInfoTrigger>();
        }
        
        public void SetUp(HeroData data, HeroCollectionView collectionView)
        {
            _isEmpty = false;
            _emptySlotImage.gameObject.SetActive(false);
            _unitBodyImage.gameObject.SetActive(true);
            _infoTrigger.SetHeroData(data);
            _data = data;
            _collectionView = collectionView;
            gameObject.SetActive(true);
            Selected = data.Selected;
            Render();
        }


        public void SetUpAsEmpty()
        {
            _isEmpty = true;
            _unitBodyImage.gameObject.SetActive(false);
            _emptySlotImage.gameObject.SetActive(true);
            _selectionFrame.gameObject.SetActive(false);
            gameObject.SetActive(true);
            
        }

        public override void Render()
        {
            _unitBodyImage.color = Game.GetUnitColor(_data.VisualIndex);
            Selected = _data.Selected;
        }

      

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isEmpty)
                return;
            if (_infoTrigger.IsHeroInfoPanelActive())
            {
                _infoTrigger.HideHeroInfo();
                return;
            }
              
            Selected = !Selected;
            if(Selected)
                _collectionView.Select(this);
            else  _collectionView.Deselect(this);
        }

    
    }
}