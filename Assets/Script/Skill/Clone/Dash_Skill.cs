namespace Script.Skill.Clone
{
    public class Dash_Skill : Skill
    {
        public bool dash = false;
        public bool dashStartMirage = false;
        public bool dashArriveMirage = false;

        public override bool CanUseSkill()
        {
            return dash && base.CanUseSkill();
        }

        protected override void UseSkill()
        {
            if (dash) 
                base.UseSkill();
        }

        public void UnlockDash()
        {
            dash = true;
        }

        public void UnlockDashStartMirage()
        {
            dashStartMirage = true;
        }

        public void UnlockDashArriveMirage()
        {
            dashArriveMirage = true;
        }
    }
}
