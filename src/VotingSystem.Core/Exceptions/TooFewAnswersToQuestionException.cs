using SharpDomain.Core;

namespace VotingSystem.Core.Exceptions
{
    internal class TooFewAnswersToQuestionException : DomainException
    {
        public TooFewAnswersToQuestionException(int minimumAnswers) 
            : base($"question must have at least {minimumAnswers} answers")
        {
        }
    }
}