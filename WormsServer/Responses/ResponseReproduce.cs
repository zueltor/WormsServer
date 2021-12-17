namespace WormsServer.Responses
{
    internal class ResponseReproduce : IResponse
    {
        public Position Step { get; private set; }

        public ResponseReproduce(Position step)
        {
            Step = step;
        }
    }
}