@owner=jagudelos @testplan=55104 @testsuite=55108 @ui @S14 @MVP2and3
Feature: PagesUnavailable
As a user, I need a visual interface that lets me know when pages are not available
@testcase=57963
Scenario Outline: Verify TRUE user is able to see 403 Error Page if the user doesn't have access to resources
Given I am logged in as "consulta"
Then I validate below error details are displayed
| key   | value                                                                                        |
| code  | 403                                                                                          |
| title | No Autorizado                                                                                |
| desc  | Lo sentimos, con las credenciales proporcionadas no tiene los permisos para ver esta página. |
@testcase=57964 @BVT2
Scenario Outline: Verify TRUE User is able to see 404 Error page if the user navigates to a non-existent page
Given I am logged in as "admin"
When I hit the "<InvalidURL>" URL directly
Then I validate below error details are displayed
| key   | value                                                                                                            |
| code  | 404                                                                                                              |
| title | Página no encontrada                                                                                             |
| desc  | Lo sentimos, esta página no existe, la dirección es incorrecta, ha cambiado, o temporalmente no está disponible. |

Examples:
| InvalidURL        |
| InvalidExceptions |
| InvalidContracts  |
| InvalidFileUpload |
| InvalidCutOff     |
@testcase=57965 @BVT2
Scenario Outline: Verify TRUE user is able to see the 500 Error page If an error occurs
Given I am logged in as "admin"
When I hit the "InternalServerError" URL directly
Then I validate below error details are displayed
| key   | value                                                                                                                                                                                                                                               |
| code  | 500                                                                                                                                                                                                                                                 |
| title | Error inesperado                                                                                                                                                                                                                                    |
| desc  | Lo sentimos, se produjo un error al procesar esta solicitud. Por favor vuelva a intentarlo mas tarde, si el problema persiste reporte la falla en la línea de atención, en el portal de autoservicio, por correo electrónico o con el chatbot TICO. |
| link  | Más información                                                                                                                                                                                                                                     |
@testcase=57966  @BVT2
Scenario Outline: Verify TRUE user is able to see the 500 error window and clicked on the details link "Más Información"
Given I am logged in as "admin"
When I hit the "InternalServerError" URL directly
When I click on Más información "link"
Then I validate below options available under "Canales de atención 24/7" section of Attention Model pop-up "Modelo de atención TRUE"
| key                 |
| Portal autoservicio |
| Chatbot TICO        |
| Línea de atención   |
| Correo electrónico  |
Then I validate "Línea de atención" option has "2345000" displayed from system parameters
Then I validate "Línea de atención" option has phone number with extension as "2345000 Opciones 4-4-1" displayed
Then I validate "Correo electrónico" option has "servicedesk@ecopetrol.com.co" displayed from system parameters
Then I validate below report steps with descriptions are displayed under "Pasos del reporte" section of Attention Model pop-up
| key               | value                                                          |
| Solicitud         | El usuario envía un correo o hace una llamada                  |
| Canal de atención | Atencion a través de los canales de Service Desk.              |
| Ticket            | Se genera ticket de atención al caso                           |
| Soporte           | Se realiza el soporte ya sea telefónico o de manera presencial |
| Notificación      | Se genera una notificación automática                          |
| Solución del caso | Es importante documentar y detallar la solución del caso       |
