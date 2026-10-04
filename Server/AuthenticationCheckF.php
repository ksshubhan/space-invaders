<?php
session_start();

// Check if the user is logged in
if (!isset($_SESSION['user_id'])) 
{
    // User not logged in
    echo "False";
} 
else 
{
    // User is logged in
    echo "True";
}
?>
