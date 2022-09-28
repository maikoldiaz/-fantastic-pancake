const ContractFactory = artifacts.require("ContractFactory");

contract('ContractFactory', (accounts) => {
    let contract;
    let contractCreator = accounts[0];

    beforeEach(async function () {
        contract = await ContractFactory.new({ from: contractCreator, gas: 3000000 });
    });

    it('has no contract default', async function () {
        let contractMetadata = await contract.Get.call('Node', 1);

        expect(contractMetadata.ContractName).to.equal('');
        expect(contractMetadata.ContractAbi).to.equal('');
        expect(contractMetadata.ContractCreationDate).to.equal('');
        expect(contractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000000');
        expect(toNumbers(contractMetadata.ContractVersion)).to.equal(0);
    });

    it('has no latest contract if no contract is registered', async function () {
        let contractMetadata = await contract.GetLatest.call('Node');

        expect(contractMetadata.ContractName).to.equal('');
        expect(contractMetadata.ContractAbi).to.equal('');
        expect(contractMetadata.ContractCreationDate).to.equal('');
        expect(contractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000000');
        expect(toNumbers(contractMetadata.ContractVersion)).to.equal(0);
    });

    it('Register and verify the contractmetadata exists using contractname and version', async function () {
        await contract.Register('Node', 'NodeAbi', '2/2/2020', '0x0000000000000000000000000000000000000125');
        let contractMetadata = await contract.Get.call('Node', 1);

        expect(contractMetadata.ContractName).to.equal('Node');
        expect(contractMetadata.ContractAbi).to.equal('NodeAbi');
        expect(contractMetadata.ContractCreationDate).to.equal('2/2/2020');
        expect(contractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(contractMetadata.ContractVersion)).to.equal(1);
    });

    it('Register and get the latest contract', async function () {
        await contract.Register('Node', 'NodeAbi1', '1/2/2020', '0x0000000000000000000000000000000000000124');
        await contract.Register('Node', 'NodeAbi2', '2/2/2020', '0x0000000000000000000000000000000000000125');

        let contractMetadata = await contract.GetLatest.call('Node');

        expect(contractMetadata.ContractName).to.equal('Node');
        expect(contractMetadata.ContractAbi).to.equal('NodeAbi2');
        expect(contractMetadata.ContractCreationDate).to.equal('2/2/2020');
        expect(contractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(contractMetadata.ContractVersion)).to.equal(2);
    });

    it('Register and get contracts by version', async function () {
        await contract.Register('Node', 'NodeAbi1', '1/2/2020', '0x0000000000000000000000000000000000000124');
        await contract.Register('Node', 'NodeAbi2', '2/2/2020', '0x0000000000000000000000000000000000000125');

        let contractMetadataVersionOne = await contract.Get.call('Node', 1);

        expect(contractMetadataVersionOne.ContractName).to.equal('Node');
        expect(contractMetadataVersionOne.ContractAbi).to.equal('NodeAbi1');
        expect(contractMetadataVersionOne.ContractCreationDate).to.equal('1/2/2020');
        expect(contractMetadataVersionOne.ContractAddress).to.equal('0x0000000000000000000000000000000000000124');
        expect(toNumbers(contractMetadataVersionOne.ContractVersion)).to.equal(1);

        let contractMetadataVersionTwo = await contract.Get.call('Node', 2);

        expect(contractMetadataVersionTwo.ContractName).to.equal('Node');
        expect(contractMetadataVersionTwo.ContractAbi).to.equal('NodeAbi2');
        expect(contractMetadataVersionTwo.ContractCreationDate).to.equal('2/2/2020');
        expect(contractMetadataVersionTwo.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(contractMetadataVersionTwo.ContractVersion)).to.equal(2);
    });

    it('Register different contacts with multiple versions and get latest for each registered contract.', async function () {

        // Node Contract
        await contract.Register('Node', 'NodeAbi1', '1/2/2020', '0x0000000000000000000000000000000000000124');
        await contract.Register('Node', 'NodeAbi2', '2/2/2020', '0x0000000000000000000000000000000000000125');

        let nodeContractMetadata = await contract.GetLatest.call('Node');
        expect(nodeContractMetadata.ContractName).to.equal('Node');
        expect(nodeContractMetadata.ContractAbi).to.equal('NodeAbi2');
        expect(nodeContractMetadata.ContractCreationDate).to.equal('2/2/2020');
        expect(nodeContractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(nodeContractMetadata.ContractVersion)).to.equal(2);

        // Movement Contract
        await contract.Register('Movement', 'MovementAbi1', '1/2/2020', '0x0000000000000000000000000000000000000124');
        await contract.Register('Movement', 'MovementAbi2', '2/2/2020', '0x0000000000000000000000000000000000000125');

        let movementContractMetadata = await contract.GetLatest.call('Movement');
        expect(movementContractMetadata.ContractName).to.equal('Movement');
        expect(movementContractMetadata.ContractAbi).to.equal('MovementAbi2');
        expect(movementContractMetadata.ContractCreationDate).to.equal('2/2/2020');
        expect(movementContractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(movementContractMetadata.ContractVersion)).to.equal(2);

        // Inventory Contract
        await contract.Register('Inventory', 'InventoryAbi1', '1/2/2020', '0x0000000000000000000000000000000000000124');
        await contract.Register('Inventory', 'InventoryAbi2', '2/2/2020', '0x0000000000000000000000000000000000000125');

        let inventoryContractMetadata = await contract.GetLatest.call('Inventory');
        expect(inventoryContractMetadata.ContractName).to.equal('Inventory');
        expect(inventoryContractMetadata.ContractAbi).to.equal('InventoryAbi2');
        expect(inventoryContractMetadata.ContractCreationDate).to.equal('2/2/2020');
        expect(inventoryContractMetadata.ContractAddress).to.equal('0x0000000000000000000000000000000000000125');
        expect(toNumbers(inventoryContractMetadata.ContractVersion)).to.equal(2);
    });

    function toNumbers(bigNumber) {
        return bigNumber.toNumber();
    }
});