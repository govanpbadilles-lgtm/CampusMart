Core Architecture Changes
Admin Role Modifications
KEEP & ENHANCE:

Floor & Stalls Management (admin creates/manages floors and stalls)
Student Directory
System Settings
Add: Store Oversight (if not already functional for monitoring)

REMOVE COMPLETELY:

Listing Approvals (entire seller approval workflow)
Rental Units
Academic Needs
All associated controllers, views, models, and database tables

User/Student Role Modifications
KEEP:

Dashboard
My Cart
Saved Items
Purchase History
Account Settings

REMOVE COMPLETELY:

Post Listing
My Listings
Study Resources
All associated controllers, views, models, and database tables

New System Flow
Stall & Product Management

Admin creates Floors → Each floor contains multiple stalls
Admin creates Stalls → Assigns to specific floor
Inside each Stall: Admin or designated stall manager can:

Create product categories
Add/edit/delete items
Manage inventory
Set prices



User Flow (Students)

Browse marketplace by floor/stall/category
Search for products using searchbar
Add items to cart
Complete transactions
View purchase history
Save favorite items

Required Features to Implement/Retain
1. Transaction System

Complete checkout process
Payment simulation (no real payment gateway needed)
Order confirmation
Transaction history for both admin and users

2. Account Management

User registration/login (ASP.NET Core Identity)
Profile picture upload functionality
Edit profile information
Password management

3. Search Functionality

Global searchbar on user dashboard
Search by: product name, category, stall name
Display search results with relevant product details

4. Admin Oversight

View all transactions
Monitor student activities
Manage user accounts (Student Directory)
View sales reports per stall/floor

File & Folder Cleanup Instructions
Remove These Completely:
Controllers/
- SellerController.cs (if exists)
- ListingApprovalController.cs
- RentalUnitsController.cs
- AcademicNeedsController.cs
- StoreOversightController.cs (unless repurposed)

Areas/
- Remove any Seller-specific areas

Models/
- ListingApproval.cs
- RentalUnit.cs
- AcademicNeed.cs
- Remove seller-specific fields from ApplicationUser

Views/
- All views related to removed features
- Seller approval workflows
- Post listing pages

Migrations/
- Keep only the latest migration after cleanup
Database Schema Changes:
sql-- Remove tables:
DROP TABLE ListingApprovals;
DROP TABLE RentalUnits;
DROP TABLE AcademicNeeds;

-- Remove from ApplicationUser:
ALTER TABLE AspNetUsers DROP COLUMN IsSellerApproved;
ALTER TABLE AspNetUsers DROP COLUMN SellerApplicationDate;
(and any other seller-related columns)

-- Ensure these exist:
Floors (Id, Name, Description, Building)
Stalls (Id, FloorId, StallNumber, StallName, Description)
Categories (Id, StallId, CategoryName)
Products (Id, CategoryId, StallId, Name, Description, Price, Stock, ImageUrl)
Transactions (Id, UserId, ProductId, Quantity, TotalPrice, TransactionDate, Status)
Cart (Id, UserId, ProductId, Quantity)
SavedItems (Id, UserId, ProductId)
Simplified Role System
Remove: Seller role entirely
Keep:

Admin - Full system control
User/Student - Browse and purchase only

Implementation Priority

✅ Remove all seller-related code and database tables
✅ Implement Floor → Stall → Category → Product hierarchy
✅ Add profile picture upload to account management
✅ Implement global searchbar
✅ Build transaction/checkout system
✅ Clean up navigation menus (both admin and user sidebars)
✅ Update routing to remove deleted endpoints
✅ Test all remaining features

Expected Output
A simplified CampusMart with:

Clean, readable codebase
Only essential marketplace features
Admin manages all product listings via stall system
Students browse, search, and purchase
Simple transaction tracking
No seller approval complexity