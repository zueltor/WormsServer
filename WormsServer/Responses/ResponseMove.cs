namespace WormsServer.Responses
{
    internal class ResponseMove : IResponse
    {
        public Position Step { get; private set; }

        public ResponseMove(Position step)
        {
            Step = step;
        }
    }
}