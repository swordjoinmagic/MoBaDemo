namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("The random probability task will return success when the random probability is above the succeed probability. It will otherwise return failure.")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=33")]
    public class RandomProbability : Conditional
    {
        [Tooltip("The chance that the task will return success")]
        public SharedFloat successProbability = 0.5f;
        [Tooltip("Seed the random number generator to make things easier to debug")]
        public SharedInt seed;
        [Tooltip("Do we want to use the seed?")]
        public SharedBool useSeed;

        private System.Random random;

        public override void OnAwake()
        {
            // If specified, use the seed provided.
            if (useSeed.Value) {
                random = new System.Random(seed.Value);
            } else {
                random = new System.Random();
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Return success if random value is less than the success probability. Otherwise return failure.
            float randomValue = (float)random.NextDouble();
            if (randomValue < successProbability.Value) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            successProbability = 0.5f;
            seed = 0;
            useSeed = false;
        }
    }
}