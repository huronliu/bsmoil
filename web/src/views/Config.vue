<template>
  <v-container fluid style="padding-left:0;padding-right:0;">
    <mobile-header :title="pageTitle">
    </mobile-header>
    <v-text-field label="Search" v-model="query" prepend-inner-icon="search" solo style="margin:5px;" hide-details @input="startSearch" maxlength=100></v-text-field>
    <div v-for="(config, index) in filteredItems" :key="index">
        <v-card class="card-style" v-if="(config.display && config.type == 'String')">
          <v-text-field v-model.lazy="config.value" :label="config.label" @input="fieldInput(config)" maxlength=256>
            <v-icon medium :color="(config.changed?'green':'')" slot="append" @click.native="update(config)">save</v-icon>
          </v-text-field>
          <v-text-field v-if="config.displayText" v-model.lazy="config.displayText" label="Display Text" @input="config.displayTextChanged = true;" maxlength=256>
            <v-icon medium :color="(config.displayTextChanged?'green':'')" slot="append" @click.native="update(config)">save</v-icon>
          </v-text-field>
        </v-card>
        <v-card class="card-style" v-if="(config.display && (config.type == 'Long' || config.type == 'Integer'))">
          <v-text-field v-model.lazy="config.value" :label="config.label" @input="fieldInput(config)" type="number" maxlength=64>
            <v-icon medium :color="(config.changed?'green':'')" slot="append" @click.native="update(config)">save</v-icon>
          </v-text-field>
        </v-card>
        <v-card class="card-style" v-if="(config.display && config.type == 'Url')">
          <v-text-field v-model.lazy="config.value" :label="config.label" @input="fieldInput(config)" type="url" maxlength=256>
            <v-icon medium :color="(config.changed?'green':'')" slot="append" @click.native="update(config)">save</v-icon>
          </v-text-field>
        </v-card>
        <v-card class="card-style" v-if="(config.display && config.type == 'Boolean')">
          <v-checkbox v-model.lazy="config.value" :label="config.label" @change="config.changed=true;update(config)"></v-checkbox>
        </v-card>
        <v-card class="card-style" v-if="(config.display && config.type == 'Textarea')">
          <v-textarea v-model.lazy="config.value" :label="config.label" @input="fieldInput(config)" readonly>
            <v-icon medium slot="append" @click.native="dialogTextAreaEditor=true;selectedConfig=config;">edit</v-icon>
          </v-textarea>
        </v-card>
        <v-card class="card-style" v-if="(config.display && config.type == 'Dropdown' && config.key == 'setting_default_country_code')">
          <v-autocomplete
            v-model.lazy="config.value"
            :items="countries"
            :label="config.label"
            item-text="name" 
            item-value="countryId" 
            @change="config.changed=true;update(config)"
          clearable></v-autocomplete>
        </v-card>
        <v-card class="card-style" v-if="(config.display && config.type == 'Dropdown' && config.key != 'setting_default_country_code')">
          <v-text-field v-model.lazy="config.value" :label="config.label" @input="config.changed = true;" maxlength=256>
            <v-icon medium :color="(config.changed?'green':'')" slot="append" @click.native="update(config)">save</v-icon>
          </v-text-field>
        </v-card>
    </div>
    <v-dialog v-model='dialogTextAreaEditor' v-if="selectedConfig" content-class="dialog-m0" persistent>
      <v-card>
        <v-card-title>{{selectedConfig.label}}</v-card-title>
        <v-card-text class="expanded-dialog-text">
          <v-textarea v-model.lazy="selectedConfig.value" @input="selectedConfig.changed = true;" hide-details auto-grow>
          </v-textarea>
        </v-card-text>
        <v-divider></v-divider>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="dialogTextAreaEditor = false;restoreItem(selectedConfig.key);selectedConfig=null;">Cancel</v-btn>
          <v-btn color="primary" @click="dialogTextAreaEditor = false;update(selectedConfig);selectedConfig=null;" :disabled="!selectedConfig.changed">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import _ from "lodash";
import directEnrollCache from "@/modules/directEnrollCache";
export default {
  name: "Config",
  data() {
    return {
      pageTitle: "",
      originKeyValues: null,
      configItems: null,
      query: null,
      filteredItems: null,
      debouncedQuery: null,
      dialogTextAreaEditor: true,
      selectedConfig: null
    };
  },
  computed: {
    countries: {
      get() {
        return directEnrollCache.getCountryList();
      }
    }
  },
  methods: {
    fieldInput(config) {
      config.changed = this.originKeyValues[config.key] != config.value;
    },
    update(config) {
      if (!config.changed) {
        return;
      }
      const newValue =
        config.value === undefined || config.value === null ? "" : config.value;
      const key = config.key;
      const displayText = config.displayText;
      this.$utils.showLoading();
      var request = { Key: key, Value: newValue, DisplayText: displayText };
      this.$http
        .post("/api/config/update", request)
        .then(result => {
          var message = result.data.Success
            ? this.$t("Config.updateSuccess")
            : this.$t("Config.updateFailed");
          if (result.data.Success) {
            this.updateLocalValue(key, newValue);
            this.resetItem(key);
          }
          this.$utils.hideLoading();
          this.$utils.toast(message, 1000);
        })
        .catch(error => {
          var errorMsg = this.$t("common.error") + ": ";
          if (error.response) {
            if (error.response.status === 401) {
              errorMsg += this.$t("common.unauthorized");
            }
          } else if (error.message) {
            errorMsg += error.message;
          }
          this.$utils.hideLoading();
          this.$utils.toast(errorMsg);
        });
    },
    startSearch() {
      this.debouncedQuery();
    },
    performSearch() {
      console.log("query configs matching: ", this.query);
      let query = this.query ? _.trim(this.query) : "";
      if (!query) {
        this.filteredItems = this.configItems;
      } else {
        this.filteredItems = _.filter(this.configItems, o => {
          if (o) {
            let match = false;
            if (o.label) {
              match = o.label.toLowerCase().indexOf(query.toLowerCase()) >= 0;
            }
            if (!match) {
              match = o.key.toLowerCase().indexOf(query.toLowerCase()) >= 0;
            }

            return match;
          } else {
            return false;
          }
        });
        console.log("found %d configs", this.filteredItems.length);
      }
    },
    resetItem(key) {
      var index = _.findIndex(this.configItems, { key: key });
      var config = this.configItems[index];
      config.changed = false;
      this.$set(this.configItems, index, config);
    },
    restoreItem(key) {
      let originalValue = this.originKeyValues[key];
      var index = _.findIndex(this.configItems, { key: key });
      var config = this.configItems[index];
      config.changed = false;
      config.value = originalValue;
      this.$set(this.configItems, index, config);
    },
    updateLocalValue(key, newValue) {
      this.originKeyValues[key] = newValue;
    }
  },
  beforeMount: function() {
    this.pageTitle = this.$t("Config.pageTitle");
  },
  activated: function() {
    this.$utils.showLoading();
    this.$http
      .get("/api/config/list?scope=3")
      .then(result => {
        var items = result.data;
        var sortedKeys = _.sortBy(
          _.keys(items),
          key => items[key].displayGroup
        );
        this.configItems = _.map(sortedKeys, key => items[key]);
        this.filteredItems = this.configItems;

        var clonedItems = new Object();
        _.forEach(items, function(value2, key2) {
          clonedItems[key2] = items[key2].value;
        });
        this.originKeyValues = clonedItems;

        this.debouncedQuery = _.debounce(this.performSearch, 1000);

        this.$utils.hideLoading();
      })
      .catch(error => {
        var errorMsg = this.$t("common.error") + ": " + error.message;
        this.$utils.hideLoading();
        this.$utils.toast(errorMsg);
      });
  },
  beforeRouteEnter(to, from, next) {
    directEnrollCache
      .initialize()
      .then(() => {
        next();
      })
      .catch(() =>
        alert(
          "Unable to initialize the languages, country, state or communication method."
        )
      );
  }
};
</script>
<style scoped>
.card-style {
  padding: 16px;
  margin: 10px 5px;
  padding-bottom: 0px;
}
.expanded-dialog-text {
  padding: 0 16px;
  max-height: 70vh;
  overflow-y: scroll;
}
@media only screen and (max-width: 500px) {
  .expanded-dialog-text {
    max-height: 60vh;
  }
}
.v-input__append-inner {
  cursor: hand;
}
</style>
<style>
@media only screen and (max-width: 500px) {
  .dialog-m0 {
    margin: 5px;
  }
}
</style>
