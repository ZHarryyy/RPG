public class Enemy_ExecutionerTriggers : Enemy_AnimationTriggers
{
    private Enemy_Executioner enemyExecutioner => GetComponentInParent<Enemy_Executioner>();

    private void Relocate() => enemyExecutioner.FindPosition();

    private void MakeInvisible() => enemyExecutioner.fx.MakeTransparent(true);

    private void MakeVisible() => enemyExecutioner.fx.MakeTransparent(false);
}
