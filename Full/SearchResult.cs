namespace Full
{
    public class SearchResult
    {
        public TreeNode Node { get; }
        public int NodesProcessed { get; }

        public SearchResult(TreeNode node, int nodesProcessed)
        {
            Node = node;
            NodesProcessed = nodesProcessed;
        }
    }
}
