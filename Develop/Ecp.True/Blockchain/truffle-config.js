//const HDWalletProvider = require('truffle-hdwallet-provider');
const fs = require('fs');
const mnemonic = "empower imitate sweet maid light blush limit chest swap say core twin"

module.exports = {
  networks: {
    development: {
      host: "127.0.0.1",
      port: 8545,
      network_id: "*"
    },
    ganache: {
      host: "127.0.0.1",
      port: 7545,
      network_id: "5777"
    }
    //abs_amancns_testcnsmember: {
    //  network_id: "*",
    //  gas: 0,
    //  gasPrice: 0,
    //    provider: new HDWalletProvider(mnemonic, "https://testcnsmember.blockchain.azure.com:3200/a7bsvljrUmlFuCLVHb6i9jdC")
    //}
  },
  mocha: {},
  compilers: {
    solc: {
      version: "0.5.0"
    }
  }
};
