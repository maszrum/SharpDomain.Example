using SharpDomain.Core;

namespace VotingSystem.Core.Exceptions
{
    internal class TooFewAnswersInQuestionException : DomainException
    {
        public TooFewAnswersInQuestionException(int minimumAnswers) 
            : base($"question must have at least {minimumAnswers} answers")
        {
        }
    }
}