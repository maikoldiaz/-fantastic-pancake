@sharedsteps=4013 @owner=jagudelos @testsuite=31121 @testplan=31102 @ui @parallel=false
Feature: ManageCategoryElementConfigurationWithColorAndIcon
In order to include the color and icon of an element
As a TRUE system administrator
I need the element configuration to be extended

Background: Login
Given I am logged in as "admin"
@testcase=33805 @bvt1.5
Scenario: Validate to select a color from the list and the color selected is displaying in the Color field for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
Then validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
@testcase=33806 @bvt1.5
Scenario: Validate to select a color from the list and the color selected is displaying in the Color field for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
Then validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
@testcase=33807
Scenario: Validate color palette is closed once the color is selected for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
Then I should not see "Picker" "Overlay" "color"
@testcase=33808
Scenario: Validate color palette is closed once the color is selected for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
Then I should not see "Picker" "Overlay" "color"
@testcase=33809
Scenario: Validate color palette is closed and no color selected when click on outside of the palette for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I click on "Picker" "Overlay" "color"
And I should not see "Picker" "Overlay" "color"
Then validate no color is displaying in "Picker" "Control" "color"
@testcase=33810
Scenario: Validate color palette is closed and no color selected when click on outside of the palette for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I click on "Picker" "Overlay" "color"
And I should not see "Picker" "Overlay" "color"
Then validate no color is displaying in "Picker" "Control" "color"
@testcase=33811
Scenario: Validate color palette is closed and previous color selected when click on outside of the palette for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I should not see "Picker" "Overlay" "color"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I click on "Picker" "Overlay" "color"
And I should not see "Picker" "Overlay" "color"
Then validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
@testcase=33812
Scenario: Validate color palette is closed and previous color selected when click on outside of the palette for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I should not see "Picker" "Overlay" "color"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I click on "Picker" "Overlay" "color"
And I should not see "Picker" "Overlay" "color"
Then validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
@testcase=33813
Scenario: Validate icon modal window for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on Icon Search Button
Then I should see "Icon" "modal"
And validate available icons are displayed
@testcase=33814
Scenario: Validate icon modal window for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on Icon Search Button
Then I should see "Icon" "modal"
And validate available icons are displayed
@testcase=33815
Scenario: Validate icons are selectable for create category element
When I navigate to "Category Elements" page
And I click on "Create Element" "button"
And I select "Sistema" from "Category"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
Then click on "Barril" icon
And I click on "Submit" "Icon" "button"
And validate selected icon "Barril" is displayed in "icon" "textbox"
@testcase=33816
Scenario: Validate icons are selectable for edit category element
Given I have "Sistema" category element in the system
When I navigate to "Category Elements" page
And I search for existing "Sistema" record using filter
And I click on "Elements" "Edit" "link" of any record
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
Then click on "Barril" icon
And I click on "Submit" "Icon" "button"
And validate selected icon "Barril" is displayed in "icon" "textbox"
@testcase=33817 @bvt1.5 @output=QueryAll(GetCategories) @version=2
Scenario: Create category element with icon and color
Given I have "Transport Category" in the system
When I navigate to "Category Elements" page
And I click on "CreateElement" "button"
And I should see "Create Element" interface
And I select any "Category" from "Category" "dropdown"
And I provide value for "element" "name" "textbox"
And I provide value for "element" "description" "textarea"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
Then validate "CategoryElement" saved with "Barril" and "9c27b0"
@testcase=33818 @bvt1.5
Scenario: Update category element with icon and color
Given I have category element in the system
When I navigate to "Category Elements" page
And I search for existing "CategoryElement" record using filter
And I click on "Elements" "Edit" "link"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
Then validate "CategoryElement" saved with "Barril" and "9c27b0"
@testcase=33819 @bvt1.5 @output=QueryAll(GetCategories)
Scenario: Create category element with color already assigned to the element of same category
Given I have "Transport Category" in the system
When I navigate to "Category Elements" page
And I click on "CreateElement" "button"
And I should see "Create Element" interface
And I select any "Category" from "Category" "dropdown"
And I provide value for "element" "name" "textbox"
And I provide value for "element" "description" "textarea"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
And validate "CategoryElement" saved with "Barril" and "9c27b0"
And I click on "CreateElement" "button"
And I should see "Create Element" interface
And I select any "Category" from "Category" "dropdown"
And I provide value for "element" "name" "textbox"
And I provide value for "element" "description" "textarea"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
Then validate the error message "El color seleccionado lo tiene asignado el elemento [Nombre del elemento que tiene asignado el color]."
@testcase=33820 @bvt1.5 @output=QueryAll(GetCategories)
Scenario: Update category element with color already assigned to the element of same category
Given I have "Transport Category" in the system
When I navigate to "Category Elements" page
And I click on "CreateElement" "button"
And I should see "Create Element" interface
And I select any "Category" from "Category" "dropdown"
And I provide value for "element" "name" "textbox"
And I provide value for "element" "description" "textarea"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
And validate "CategoryElement" saved with "Barril" and "9c27b0"
And I click on "CreateElement" "button"
And I should see "Create Element" interface
And I select any "Category" from "Category" "dropdown"
And I provide value for "element" "name" "textbox"
And I provide value for "element" "description" "textarea"
And I click on Icon Search Button
And I should see "Icon" "modal"
And validate available icons are displayed
And click on "Barril" icon
And I click on "Submit" "icon"
And validate selected icon "Barril" is displayed in "icon" "textbox"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "7b1fa2" from "Picker" "Overlay" "color"
And validate selected color "7b1fa2" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
And I search for existing "CategoryElement" record using filter
And I click on "Elements" "Edit" "link"
And I click on "Picker" "Control" "color"
And I should see "Picker" "Overlay" "color"
And I select color "9c27b0" from "Picker" "Overlay" "color"
And validate selected color "9c27b0" is displaying in "Picker" "Control" "color"
And I click on "element" "submit" "button"
Then validate the error message "El color seleccionado lo tiene asignado el elemento [Nombre del elemento que tiene asignado el color]."
