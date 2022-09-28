@sharedsteps=7539 @owner=jagudelos @backend @testplan=39221 @testsuite=39238 @R1.5
Feature: ProcessPoisonedOrDeadLetteredMessages
As an Operations-user, I want to have a mechanism
to re-process poisoned/dead-lettered messages so that
the previously failed transactions are successful

Background: Login
Given I am authenticated as "admin"

@testcase=41761 @version=3
Scenario Outline: Verify OperationalCutoff and OwnershipCalculation message gets into poisoned or deadlettered queue when trasactions are failed
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
Then messages are moved into deadletter queue

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41929 @version=3
Scenario Outline: Verify File upload message gets into poisoned or deadlettered queue when trasactions are failed
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
Then messages are moved into deadletter queue

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41930 @version=3
Scenario: Verify Sap Po inventory queue message gets into poisoned or deadlettered queue when trasactions are failed
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue

@testcase=52204
Scenario: Verify Sap Po movement queue message gets into poisoned or deadlettered queue when trasactions are failed
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue


@testcase=41762 @version=3
Scenario Outline: Verify OperationalCutoff and OwnershipCalculation poisoned or dead lettered messages are copied to a new blob when transactions are failed
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
Then messages are moved into deadletter queue
And a new blob is created for exception handling
And messages are copied to blob storage

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41931 @version=3
Scenario Outline: Verify File upload poisoned or dead lettered messages are copied to a new blob when transactions are failed
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
Then messages are moved into deadletter queue
And a new blob is created for exception handling
And messages are copied to blob storage

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41932 @version=3
Scenario: Verify Sap Po inventory poisoned or dead lettered messages are copied to a new blob when transactions are failed
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue
And a new blob is created for exception handling
And messages are copied to blob storage

@testcase=52205
Scenario: Verify Sap Po movement poisoned or dead lettered messages are copied to a new blob when transactions are failed
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue
And a new blob is created for exception handling
And messages are copied to blob storage

@testcase=41763 @version=3
Scenario Outline: Verify OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are copied to newly available blob storage when transactions are failed and failure processing blob is available
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
And messages are moved into deadletter queue
And I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
Then messages are copied to new blob storage

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41933 @version=3
Scenario Outline: Verify File upload poisoned or deadlettered messages are copied to newly available blob storage when transactions are failed and failure processing blob is available
Given I have processed "<Queue>" in the system
When "<Queue>" transactions are failed
Then messages are moved into deadletter queue
And messages are copied to new blob storage

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41934 @version=3
Scenario: Verify Sap Po movement poisoned or deadlettered messages are copied to newly available blob storage when transactions are failed and failure processing blob is available
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue
And messages are copied to new blob storage

@testcase=52206
Scenario: Verify Sap Po inventory poisoned or deadlettered messages are copied to newly available blob storage when transactions are failed and failure processing blob is available
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
Then messages are moved into deadletter queue
And messages are copied to new blob storage

@testcase=41764 @version=3
Scenario Outline: Verify OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are processed when triggered through API
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "<Queue>" messages should be processed successfully

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41935 @version=3
Scenario Outline: Verify File upload poisoned or deadlettered messages are processed when triggered through API
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "<Queue>" messages should be processed successfully

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41936 @version=3
Scenario: Verify Sap Po inventory poisoned or deadlettered messages are processed when triggered through API
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Json" messages should be processed successfully

@testcase=52207
Scenario: Verify Sap Po movement poisoned or deadlettered messages are processed when triggered through API
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Json" messages should be processed successfully

@testcase=41765 @version=3
Scenario Outline: Verify OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are processed when triggered through API with multiple blob details
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for "<Queue>" message processing with more than one blob detail
Then multiple messages should be processed successfully
And multiple "<Queue>" messages should be processed successfully

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41937 @version=3
Scenario Outline: Verify File upload poisoned or deadlettered messages are processed when triggered through API with multiple blob details
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for "<Queue>" message processing with more than one blob detail
Then multiple messages should be processed successfully
And multiple "<Queue>" messages should be processed successfully

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41938 @version=3
Scenario: Verify Sap Po inventory poisoned or deadlettered messages are processed when triggered through API with multiple blob details
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for "Inventories_Json" message processing with more than one blob detail
Then multiple messages should be processed successfully
And multiple "Json" messages should be processed successfully

@testcase=52208
Scenario: Verify Sap Po movement poisoned or deadlettered messages are processed when triggered through API with multiple blob details
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for "Movements_Json" message processing with more than one blob detail
Then multiple messages should be processed successfully
And "Json" messages should be processed successfully

@testcase=41766 @version=3
Scenario Outline: Verify whether blobs are removed from container when OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are processed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "<Queue>" messages should be processed successfully
And blobs should be removed from contanier after configured interval

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41939 @version=3
Scenario Outline: Verify whether blobs are removed from container when File upload poisoned or deadlettered messages are processed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "<Queue>" messages should be processed successfully
And blobs should be removed from contanier after configured interval

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41940 @version=3
Scenario: Verify whether blobs are removed from container when Sap Po inventory poisoned or deadlettered messages are processed
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Json" messages should be processed successfully
And blobs should be removed from contanier after configured interval

@testcase=52209
Scenario: Verify whether blobs are removed from container when Sap Po movement poisoned or deadlettered messages are processed
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Json" messages should be processed successfully
And blobs should be removed from contanier after configured interval

@testcase=41767 @version=3
Scenario Outline: Verify blobs should not be removed from container when processing of OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are failed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When I trigger an API request and it is failed
Then blobs should not be removed from contanier

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41941 @version=3
Scenario Outline: Verify blobs should not be removed from container when processing of File upload poisoned or deadlettered messages are failed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When I trigger an API request and it is failed
Then blobs should not be removed from contanier

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41942 @version=3
Scenario: Verify blobs should not be removed from container when processing of Sap Po inventory poisoned or deadlettered messages are failed
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And I trigger an API request and it is failed
Then blobs should not be removed from contanier

@testcase=52210
Scenario: Verify blobs should not be removed from container when processing of Sap Po movemnet poisoned or deadlettered messages are failed
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And I trigger an API request and it is failed
Then blobs should not be removed from contanier

@testcase=41768 @version=3
Scenario Outline: Verify deadletter label is added into app insights when OperationalCutoff and OwnershipCalculation poisoned or deadlettered messages are processed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "Message received with property deadletter" is logged in app insights

Examples:
| Queue                  |
| OperationalCutoffQueue |
| OwnershipQueue         |

@testcase=41943 @version=3
Scenario Outline: Verify deadletter label is added into app insights when File upload poisoned or deadlettered messages are processed
Given I have processed "<Queue>" in the system
And "<Queue>" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
When API is triggered for message processing
Then "Message received with property deadletter" is logged in app insights

Examples:
| Queue            |
| Excel            |
| ExcelContract    |
| ExcelEvent       |
| SinoperInventory |
| SinoperMovements |

@testcase=41944 @version=3
Scenario: Verify deadletter label is added into app insights when Sap Po inventory poisoned or deadlettered messages are processed
Given I have processed Sap Po "Inventories" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Message received with property deadletter" is logged in app insights

@testcase=52211 
Scenario: Verify deadletter label is added into app insights when Sap Po movement poisoned or deadlettered messages are processed
Given I have processed Sap Po "Movements" in the system
When Sap Po "Json" transactions are failed
And poisoned or deadlettered messages are generated
And messages are copied to blob storage
And API is triggered for message processing
Then "Message received with property deadletter" is logged in app insights