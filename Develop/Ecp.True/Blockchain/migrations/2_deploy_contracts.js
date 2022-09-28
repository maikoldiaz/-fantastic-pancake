var MovementFactory = artifacts.require("./Movement/MovementsFactory.sol");
var MovementOwnerships = artifacts.require("./Movement/MovementOwnershipsFactory.sol");
var MovementOwners = artifacts.require("./Movement/MovementOwnersFactory.sol");

var InventoryProductFactory = artifacts.require("./InventoryProduct/InventoryProductsFactory.sol");
var InventoryProductOwnerships = artifacts.require("./InventoryProduct/InventoryProductOwnershipsFactory.sol");
var InventoryProductOwners = artifacts.require("./InventoryProduct/InventoryProductOwnersFactory.sol");

var NodesFactory = artifacts.require("./Admin/NodesFactory.sol");
var NodeConnectionsFactory = artifacts.require("./Admin/NodeConnectionsFactory.sol");
var UnbalancesFactory = artifacts.require("./Calculation/UnbalancesFactory.sol");
var ContractFactory = artifacts.require("./ContractFactory.sol");

module.exports = deployer => {
    deployer.deploy(MovementFactory);
    deployer.deploy(InventoryProductFactory);
    deployer.deploy(NodesFactory);
    deployer.deploy(NodeConnectionsFactory);
    deployer.deploy(UnbalancesFactory);
    deployer.deploy(ContractFactory);
    deployer.deploy(MovementOwners);
    deployer.deploy(MovementOwnerships);
    deployer.deploy(InventoryProductOwners);
    deployer.deploy(InventoryProductOwnerships);
};