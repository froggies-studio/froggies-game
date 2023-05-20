using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Behaviour
{
    public class NoteActor: StoryActor
    {
        [SerializeField] protected Button nextButton;

        public NoteActor()
        {
            // nextButton.onClick.AddListener(OnNextButtonClick);
        }


        public void OnNextButtonClick()
        {
            
        }
    }
}