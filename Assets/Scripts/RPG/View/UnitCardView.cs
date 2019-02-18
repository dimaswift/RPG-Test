using System.Collections;
using RPG.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.View
{
    public class UnitCardView : View, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler, IPointerUpHandler
    {
        public HeroState HeroState
        {
            get { return _state; }
        }

        [SerializeField] Image _unitBodyImage;
        [SerializeField] Image _emptySlotImage;
        [SerializeField] GameObject _selectionFrame;

        UnitConfig _unitConfig;
        HeroState _state;

        bool _selected;
        HeroCollectionView _collectionView;
        float _tapTime;
        Coroutine _waitForShowInfoRoutine;
        bool _isEmpty;
        
        public void SetUp(UnitConfig hero, HeroState state, HeroCollectionView collectionView)
        {
            _isEmpty = false;
            _emptySlotImage.gameObject.SetActive(false);
            _unitBodyImage.gameObject.SetActive(true);
            
            _state = state;
            _unitConfig = hero;
            _collectionView = collectionView;
            Render();
        }


        public void SetUpAsEmpty()
        {
            _isEmpty = true;
            _unitBodyImage.gameObject.SetActive(false);
            _emptySlotImage.gameObject.SetActive(true);
        }
        
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; _selectionFrame.SetActive(value); }
        }
        
        public override void Render()
        {
            _unitBodyImage.color = Game.GetUnitColor(_unitConfig.VisualIndex);
            Selected = _state.Selected;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(_isEmpty)
                return;
            StopWaitForShowInfo();
            _waitForShowInfoRoutine = StartCoroutine(WaitForShowInfo());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isEmpty)
                return;
            Selected = !Selected;
            _collectionView.OnSelectionChanged();
        }

        void StopWaitForShowInfo()
        {
            if (_waitForShowInfoRoutine != null)
            {
                StopCoroutine(_waitForShowInfoRoutine);
                _waitForShowInfoRoutine = null;
            }
        }
        
        IEnumerator WaitForShowInfo()
        {
            yield return new WaitForSeconds(3);
            _collectionView.ShowHeroInfo(this);
            StopWaitForShowInfo();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(_isEmpty)
                return;
            StopWaitForShowInfo();
        }

        public void OnPointerUp(PointerEventData eventData)
        {  
            if(_isEmpty)
                return;
            StopWaitForShowInfo();
        }
    }
}