pragma solidity ^0.5.0;

contract ContractFactory {
    constructor() public {}

    struct ContractMetadata {
        string ContractName;
        string ContractAbi;
        string ContractCreationDate;
        address ContractAddress;
        int64 ContractVersion;
    }

    mapping(string => ContractMetadata[]) AllContracts;
    mapping(address => ContractMetadata) AllAddresses;

    function Register(
        string memory contractName,
        string memory contractAbi,
        string memory contractCreationDate,
        address contractAddress
    ) public {
        ContractMetadata[] memory mappedContracts = AllContracts[contractName];
        uint256 arrayLength = mappedContracts.length;
        int64 currentVersion = 1;
        if (arrayLength > 0) {
            currentVersion = mappedContracts[arrayLength - 1].ContractVersion + 1;
        }

        ContractMetadata memory contractMetadata;

        contractMetadata.ContractName = contractName;
        contractMetadata.ContractAbi = contractAbi;
        contractMetadata.ContractCreationDate = contractCreationDate;
        contractMetadata.ContractAddress = contractAddress;
        contractMetadata.ContractVersion = currentVersion;

        AllContracts[contractName].push(contractMetadata);
        AllAddresses[contractAddress] = contractMetadata;
    }

    function Get(string memory contractName, int64 version)
        public
        view
        returns (
            string memory ContractName,
            string memory ContractAbi,
            string memory ContractCreationDate,
            address ContractAddress,
            int64 ContractVersion
        )
    {
        ContractMetadata[] memory mappedContracts = AllContracts[contractName];
        ContractMetadata memory contractMetadata;
        uint256 arrayLength = mappedContracts.length;
        for (uint256 i = 0; i < arrayLength; i++) {
            contractMetadata = mappedContracts[i];
            if (contractMetadata.ContractVersion == version) {
                return (
                    contractMetadata.ContractName,
                    contractMetadata.ContractAbi,
                    contractMetadata.ContractCreationDate,
                    contractMetadata.ContractAddress,
                    contractMetadata.ContractVersion
                );
            }
        }
    }

    function GetLatest(string memory contractName)
        public
        view
        returns (
            string memory ContractName,
            string memory ContractAbi,
            string memory ContractCreationDate,
            address ContractAddress,
            int64 ContractVersion
        )
    {
        ContractMetadata[] memory mappedContracts = AllContracts[contractName];
        uint256 arrayLength = mappedContracts.length;
        if (arrayLength > 0) {
            ContractMetadata memory contractMetadata = mappedContracts[arrayLength -
                1];
            return (
                contractMetadata.ContractName,
                contractMetadata.ContractAbi,
                contractMetadata.ContractCreationDate,
                contractMetadata.ContractAddress,
                contractMetadata.ContractVersion
            );
        }
    }

    function GetByAddress(address contractAddress)
        public
        view
        returns (
            string memory ContractName,
            string memory ContractAbi,
            string memory ContractCreationDate,
            address ContractAddress,
            int64 ContractVersion
        )
    {
        ContractMetadata memory contractMetadata = AllAddresses[contractAddress];
        return (
                contractMetadata.ContractName,
                contractMetadata.ContractAbi,
                contractMetadata.ContractCreationDate,
                contractMetadata.ContractAddress,
                contractMetadata.ContractVersion
            );
    }
}
