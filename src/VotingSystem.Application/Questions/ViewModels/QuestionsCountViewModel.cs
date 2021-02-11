using VotingSystem.Core.Questions;

namespace VotingSystem.Application.Questions.ViewModels
{
    public class QuestionsCountViewModel
    {
        public QuestionsCountViewModel(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public override string ToString() => 
            $"{nameof(Question)} objects count = {Count}";
    }
}