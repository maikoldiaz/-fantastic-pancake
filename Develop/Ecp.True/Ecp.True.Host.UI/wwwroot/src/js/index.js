import { appInitFactory } from './common/layouts/app.jsx';
import './common/services/bootstrap';

// home page
import './modules/home/index';

// admin modules
import './modules/administration/node/index';
import './modules/administration/nodeConnection/attributes/index';
import './modules/administration/nodeConnection/network/index';
import './modules/administration/category/index';
import './modules/administration/homologation/index';
import './modules/administration/exceptions/index';
import './modules/administration/transferPoints/operative/index';
import './modules/administration/transferPoints/logistics/index';
import './modules/administration/flowSettings/index';
import './modules/administration/transformSettings/index';
import './modules/administration/annulation/index';
import './modules/administration/operationalSegments/index';
import './modules/administration/blockchain/index';
import './modules/administration/registryDeviationConfiguration/index';
import './modules/administration/productConfiguration/index';
import './modules/administration/integrationManagement/index';

// transport balance modules
import './modules/transportBalance/nodeApproval/index';
import './modules/transportBalance/cutOff/index';
import './modules/transportBalance/ownership/index';
import './modules/transportBalance/fileUploads/index';
import './modules/transportBalance/contracts/index';
import './modules/transportBalance/logistics/index';
import './modules/transportBalance/nodeOwnership/index';
import './modules/transportBalance/operationalDelta/index';
import './modules/transportBalance/operativeBalanceReport/index';

// daily balance modules
import './modules/dailyBalance/contracts/index';
import './modules/dailyBalance/officialDelta/index';
import './modules/dailyBalance/officialDeltaPerNode/index';
import './modules/dailyBalance/officialNodeApproval/index';
import './modules/dailyBalance/officialLogistics/index';
import './modules/dailyBalance/reports/index';
import './modules/dailyBalance/sentToSap/index';

// report modules
import './modules/report/index';

appInitFactory();
