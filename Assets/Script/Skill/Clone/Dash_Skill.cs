namespace Script.Skill.Clone
{
    public class Dash_Skill : Skill
    {
        public bool dash = false;
        public bool dashMirage = false;
        public bool dashArriveMirage = false;
        
        protected override void UseSkill()
        {
            if (dash)
                base.UseSkill();
        }

        public void UnlockDash()
        {
            dash = true;
        }

        public void UnlockDashMirage()
        {
            dashMirage = true;
        }

        public void UnlockDashArriveMirage()
        {
            dashArriveMirage = true;
        }
    }
}
