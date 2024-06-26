using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AVL_Tree
{
    /*AVL Interface*/
    public interface AVL_Interface<E>
    {
        void insert(E obj);
        E search(int name);
        void delete(int name);
    }

    /*AVL Tree class*/
    public class AVL_Class : AVL_Interface<product>
    {
        public product root;
        public static Boolean isDeleted = false;
        public static Boolean isDuplicate = false;
        public static List<product> nodesToArray = new List<product>();

        /*Helper function for height of the tree*/
        private int height(product node)
        {
            if (node == null)
                return 0;
            return node.height;
        }

        /*A helper function for finding maximum value*/
        private int max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        /* Helper function to right rotate subtree rooted with y */
        private product rightRotate(product y)
        {
            product x = y.left;
            product temp = x.right;

            /* Perform rotation */
            x.right = y;
            y.left = temp;

            /* Update heights*/
            y.height = max(height(y.left), height(y.right)) + 1;
            x.height = max(height(x.left), height(x.right)) + 1;

            /* Return new root */
            return x;
        }

        /*Helper Method to left rotate subtree rooted with x */
        private product leftRotate(product x)
        {
            product y = x.right;
            product temp = y.left;

            /* Perform rotation */
            y.left = x;
            x.right = temp;

            /* Update heights */
            x.height = max(height(x.left), height(x.right)) + 1;
            y.height = max(height(y.left), height(y.right)) + 1;

            /* Return new root */
            return y;
        }

        /*Method for getting Balance factor of node n */
        private int getBalance(product n)
        {
            if (n == null)
                return 0;
            return height(n.left) - height(n.right);
        }

        /*Helper method to find minimum value node in a tree*/
        private product minValueNode(product node)
        {
            product current = node;
            /* loop down to find the leftmost leaf because left-most will have min value as per BST defination*/
            while (current.left != null)
                current = current.left;

            return current;
        }

        /***************************************************************************************/
        public void insert(product obj)
        {
            root = insertHelper(root, obj);
        }
        /*** Helper Method for insertion ***/
        private product insertHelper(product node, product key)
        {
            /* Perform the normal BST insertion */
            if (node == null)
            {
                root = key;
                return root;
            }

            if (key.ID < node.ID)
            {
                if (key.ID == root.ID)
                    isDuplicate = true;
                else
                    isDuplicate = false;
                node.left = insertHelper(node.left, key);
            }
            else if (key.ID > node.ID)
            {
                if (key.ID == root.ID)
                    isDuplicate = true;
                else
                    isDuplicate = false;
                node.right = insertHelper(node.right, key);
            }
            else
            {// Duplicate keys not allowed
                isDuplicate = true;
                return node;
            }

            /* Update height of this ancestor node */
            node.height = 1 + max(height(node.left), height(node.right));

            /*checking for unbalanced node*/
            int balance = getBalance(node);

            /*If the node is unbalanced then we have four cased*/
            /*Left Left case*/
            if (balance > 1 && key.ID < node.left.ID)
                return rightRotate(node);

            /*ight Right Case */
            if (balance < -1 && key.ID > node.right.ID)
                return leftRotate(node);

            /*Left Right Case*/
            if (balance > 1 && key.ID > node.left.ID)
            {
                node.left = leftRotate(node.left);
                return rightRotate(node);
            }

            /*Right Left Case*/
            if (balance < -1 && key.ID < node.right.ID)
            {
                node.right = rightRotate(node.right);
                return leftRotate(node);
            }

            /* return the unchanged node pointer */
            return node;
        }


        public void delete(int id)
        {
            root = deleteNodeHelper(root, id);
            if (isDeleted == true)
            {
                Console.WriteLine("Record Deleted !");
            }
            else
            {
                Console.WriteLine("Record not present");
            }
        }
        /*** Helper Method for Deletion ***/
        private product deleteNodeHelper(product root, int key)
        {
            /*First, we will perform normal bst delete*/
            if (root == null)
                return root;

            if (key < root.ID)
                root.left = deleteNodeHelper(root.left, key);
            else if (key > root.ID)
                root.right = deleteNodeHelper(root.right, key);
            else
            {
                /*node with only one child or no child*/
                if ((root.left == null) || (root.right == null))
                {
                    product temp = null;

                    if (temp == root.left)
                        temp = root.right;
                    else
                        temp = root.left;

                    /* If no child case */
                    if (temp == null)
                    {
                        temp = root;
                        root = null;
                    }
                    else { root = temp; } /*One child case*/
                    isDeleted = true;
                }
                else
                { /*Node with two childs*/
                    /*Get the node with minimum value*/
                    product temp = minValueNode(root.right);

                    /* Copy the inorder successor's data to this node*/
                    root.ID = temp.ID;

                    /* Delete the inorder successor */
                    root.right = deleteNodeHelper(root.right, temp.ID);
                }
            }

            /* Secondly, we have to make the node height and to balance it if it is unbalanced*/
            if (root == null)
                return root;

            /*Update the height of the currenct node*/
            root.height = max(height(root.left), height(root.right)) + 1;

            /*Check for balance factor*/
            int balance = getBalance(root);

            /*If the node is unbalanced then there are four cased*/
            /* Left Left Case */
            if (balance > 1 && getBalance(root.left) >= 0)
                return rightRotate(root);

            /* Left Right Case*/
            if (balance > 1 && getBalance(root.left) < 0)
            {
                root.left = leftRotate(root.left);
                return rightRotate(root);
            }

            /* Right Right Case */
            if (balance < -1 && getBalance(root.right) <= 0)
                return leftRotate(root);

            /*Right Left Case*/
            if (balance < -1 && getBalance(root.right) > 0)
            {
                root.right = rightRotate(root.right);
                return leftRotate(root);
            }
            return root;
        }

        public product search(int id)
        {
            product temp = search(root, id);
            return temp;
        }
        /*Helper method for search*/
        private product search(product root, int key)
        {
            //root is null or key is present at root
            if (root == null || root.ID == key)
                return root;
            // Key is greater than root's key
            if (root.ID < key)
                return search(root.right, key);
            // Key is smaller than root's key
            return search(root.left, key);
        }


        /*Inorder traversal of avl tree*/
        public void inorder()
        {
            inorderRecursion(root);
        }

        /*Helper method for in-order method*/
        private void inorderRecursion(product root)
        {
            if (root != null)
            {
                inorderRecursion(root.left);
                Console.WriteLine("ID: " + root.ID + "\nNAME: " + root.NAME+ 
                    "\nBRAND: " + root.BRAND + "\nPRICE ($): " + root.COST + "\n");
                inorderRecursion(root.right);
            }
        }

        /*AVL node to array*/
        public void nToA(product root)
        {
            if (root != null)
            {
                nToA(root.left);
                nodesToArray.Add(root);
                nToA(root.right);
            }
        }

        /*Count total records in bst*/
        public int size(product node)
        {
            if (node == null)
                return 0;
            else
                return (size(node.left) + 1 + size(node.right));
        }
    }

    /****************************************************************************************
     ********************* MAIN Driver Class ************************************************/
    class AVL
    {
        static void Main(string[] args)
        {
            AVL_Class avl = new AVL_Class();
            string fileName = "stockData.txt";

            /*From file to BST*/
            // Read file using StreamReader. Reads file line by line
            if (File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    string ln = file.ReadLine();
                    while (ln != null)
                    {
                        String[] tokens = ln.Split(",");
                        if (tokens.Length == 1)
                        {
                            product p = new product(int.Parse(tokens[0]), null, null, null);
                            avl.insert(p);
                        }
                        if (tokens.Length == 2)
                        {
                            product p = new product(int.Parse(tokens[0]), tokens[1], null, null);
                            avl.insert(p);
                        }
                        if (tokens.Length == 3)
                        {
                            product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], null);
                            avl.insert(p);
                        }
                        if (tokens.Length == 4)
                        {
                            product p = new product(int.Parse(tokens[0]), tokens[1], tokens[2], tokens[3]);
                            avl.insert(p);
                        }
                        ln = file.ReadLine();
                    }
                    file.Close();
                }
            }
            else
            {
                Console.WriteLine("File not found!");
            }           
            
            /******************* User Menu********************/

            int again;
            do
            {                
                Console.WriteLine("======================================");
                Console.WriteLine("  Welcome to Stock Management System");
                Console.WriteLine("======================================");
                Console.WriteLine("\t1. Insert new Record.");
                Console.WriteLine("\t2. Delete Record.");
                Console.WriteLine("\t3. Search Record.");
                Console.WriteLine("\t4. See all Records.");
                Console.WriteLine("\t5. Save & Exit.");
                Console.WriteLine("======================================");
                Console.Write("Enter your choice >> ");
                int a = Convert.ToInt32(Console.ReadLine());

                switch (a)
                {
                    case 1:
                        Console.WriteLine("_____________________________________");
                        Console.Write("Enter ID for Product >>");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter Name of Product >> ");
                        String name = Console.ReadLine();
                        Console.Write("Enter Brand of Product >>");
                        String brand = Console.ReadLine();
                        Console.Write("Enter Price of Product >> ");
                        String price = Console.ReadLine();

                        product cs = new product(id, name, brand, price);
                        avl.insert(cs);
                        if (AVL_Class.isDuplicate == false)
                        {
                            Console.WriteLine("\n------Inserted Successfully-------");
                        }
                        else
                        {
                            Console.WriteLine("\n``````````````````````````````````````");
                            Console.WriteLine("ID already registered, Try another.");
                        }
                        break;

                    /*To delete record by id*/
                    case 2:
                        Console.WriteLine("``````````````````````````````````````");
                        Console.Write("Enter ID to Delete >> ");
                        int idDel = Convert.ToInt32(Console.ReadLine());

                        /*Firstly to search*/
                        product temp = avl.search(idDel);
                        if (temp != null)
                        {
                            Console.WriteLine("ID: " + temp.ID + "\nNAME: " + temp.NAME +
                            "\nBRAND: " + temp.BRAND + "\nPRICE ($): " + temp.COST + "\n");
                            Console.WriteLine("\nConfirm to delete [yes(1)]");
                            int conf = Convert.ToInt32(Console.ReadLine());
                            if (conf == 1)
                            {
                                Console.WriteLine("Item Deleted!");
                                avl.delete(idDel);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Item not present");
                        }
                        break;

                    /*To search record by id*/
                    case 3:                        
                        Console.WriteLine("_____________________________________");
                        Console.Write("Enter ID to Search >> ");
                        int idSrch = Convert.ToInt32(Console.ReadLine());

                        temp = avl.search(idSrch);
                        if (temp != null)
                        {
                            Console.WriteLine("ID: " + temp.ID + "\nNAME: " + temp.NAME +
                            "\nBRAND: " + temp.BRAND + "\nPRICE ($): " + temp.COST + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Item not present");
                        }
                        break;

                    /*to retrieve all the records*/
                    case 4:
                        Console.WriteLine("_____________________________________");
                        int total = avl.size(avl.root);
                        Console.WriteLine("\tTotal Records: " + total);
                        Console.WriteLine("``````````````````````````````````````");
                        avl.inorder();

                        break;

                    /*to save records in file and exit*/
                    case 5:
                        /*First to retrieve records from bst*/
                        avl.nToA(avl.root);

                        /*Secondly, to shuffle and store in file*/
                        try
                        {
                            // Write file using StreamWriter  
                            using (StreamWriter writer = new StreamWriter(fileName))
                            {
                                //Collections.shuffle(BST_Class.nodesToArray);
                                var shuffled = AVL_Class.nodesToArray.OrderBy(x => Guid.NewGuid()).ToList();
                                for (int i = 0; i < shuffled.Count; i++)
                                {
                                    temp = AVL_Class.nodesToArray[i];
                                    writer.WriteLine(temp.ID + "," + temp.NAME + "," + temp.BRAND + "," + temp.COST);
                                }
                            }
                        }
                        catch (Exception i)
                        {
                            Console.WriteLine(i.Message);
                        }
                        /*close the application*/
                        Console.WriteLine("__________________________________________________");
                        int tot = avl.size(avl.root);
                        Console.WriteLine("\n[" + tot + "] Products Data Saved in " + fileName + " File");
                        Console.WriteLine("```````````````````````````````````````````````````");
                        return;                        
                }
                Console.WriteLine("_____________________________________");
                Console.WriteLine("\nDo you want to continue ? [yes(1)]");
                again = Convert.ToInt32(Console.ReadLine());
                Console.Clear(); /*to clear screen*/

            } while (again == 1);

        }
    }
}
