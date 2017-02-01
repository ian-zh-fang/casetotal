namespace zh.fang.module
{
    public class ConfigModule
    {
        public data.entity.Config FetchHomeTitle()
        {
            using (var handler = new handle.ConfigHandler())
            {
                return FetchHomeTitle(handler);
            }
        }

        public bool ChangeHomeTitle(string title)
        {
            using (var handler = new handle.ConfigHandler())
            {
                var cfg = FetchHomeTitle(handler);
                cfg.Data = title;
                return handler.Update(cfg);
            }
        }

        private data.entity.Config FetchHomeTitle(handle.ConfigHandler handler)
        {
            return handler.FetchOne(1);
        }
    }
}
