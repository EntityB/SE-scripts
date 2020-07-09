// Center of the mass calculation

public Vector3D getCoT(List<Vector3I> optMassBlocks) {
    Vector3D CoT = new Vector3D();
    var massPositions = new List<Vector3I>(massBlocks.Count);
    foreach (IMyVirtualMass massBlock in massBlocks)
    {
        vitrualMass += massBlock.VirtualMass;
        CoT += massBlock.Position;
        massPositions.Add(massBlock.Position);
    }
    CoT /= massBlocks.Count;
    return CoT
}