module.exports = {
  rootDir: ".",
  verbose: true,
  moduleFileExtensions: ["js", "vue"],
  transform: {
    ".+\\.(css|styl|less|sass|scss|svg|png|jpg|ttf|woff|woff2)$":
      "jest-transform-stub",
    ".*\\.(vue)$": "<rootDir>/node_modules/vue-jest",
    "^.+\\.js$": "<rootDir>/node_modules/babel-jest"
  },
  moduleNameMapper: {
    "^@/(.*)$": "<rootDir>/src/$1"
  },
  moduleDirectories: ["node_modules"],
  testMatch: ["**/tests/unit/**/*.spec.js"],
  testPathIgnorePatterns: ["/node_modules/"],
  snapshotSerializers: ["jest-serializer-vue"],
  setupTestFrameworkScriptFile: "<rootDir>/tests/unit/setup.js",
  collectCoverage: true,
  coverageReporters: ["cobertura", "html"],
  coverageDirectory: "<rootDir>/coverage",
  collectCoverageFrom: [
    "src/**/*.{js,vue}",
    "!src/main.js",
    "!src/router.js",
    "!src/router/**",
    "!src/i18n/**",
    "!src/plugins/**",
    "!src/assets/**",
    "!**/node_modules/**"
  ],
  testURL: "http://localhost:3000/"
};
