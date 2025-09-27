#!/bin/bash
dir="./scripts"

# Restore script
chmod +x "$dir/Restore.sh"
echo "Restore.sh Active"

# Build script
chmod +x "$dir/Build.sh"
echo "Build.sh Active"

#Run script
chmod +x "$dir/Run.sh"
echo "Run.sh Active"

#Publish script
chmod +x "$dir/Publish.sh"
echo "Publish.sh Active"

# Migration script
chmod +x "$dir/Migrate.sh"
echo "Migrate.sh Active"

# Remove migration script
chmod +x "$dir/Remove-Migration.sh"
echo "Remove-Migration.sh Active"

# Update database script
chmod +x "$dir/Update-Database.sh"
echo "Update-Database.sh Active"