namespace DiffLib;

internal class AlignmentNode
{
    public AlignmentNode(DiffOperation operation, double similarity, int nodeCount, AlignmentNode? next)
    {
        Operation = operation;
        Similarity = similarity;
        NodeCount = nodeCount;
        Next = next;
    }

    public DiffOperation Operation
    {
        get;
    }

    public double Similarity
    {
        get;
    }

    public double AverageSimilarity => NodeCount == 0 ? 0.0 : Similarity / NodeCount;

    public int NodeCount
    {
        get;
    }

    public AlignmentNode? Next
    {
        get;
    }
}