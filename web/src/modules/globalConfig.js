import cache from "@/cache";
import axios from "axios";

function GlobalConfig() {}
GlobalConfig.prototype.init = function() {
  cache.setDataProvider("configs", () =>
    axios.get("/api/config/list?scope=3", { data: {} }).then(res => res.data)
  );

  return cache.initialize(["configs"]);
};
GlobalConfig.prototype.clear = function() {
  cache.clear("configs");
};

export default new GlobalConfig();
