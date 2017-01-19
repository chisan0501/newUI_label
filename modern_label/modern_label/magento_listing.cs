using modern_label.org.connectall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class magento_listing
    {

        public magento_listing()
        {


        }

        public void get_exisiting(RefrubHistoryObj spec)
        {
            MagentoService mservice = new MagentoService();
            String mlogin = mservice.login("admin", "Interconnection123!");

            string[] sku_arr = { spec.sku };
            var product = mservice.catalogInventoryStockItemList(mlogin, sku_arr);

            if (product.Length <= 0)
            {
                create_listing(spec);
            }

        }

        public void create_listing (RefrubHistoryObj spec)
        {
            MagentoService mservice = new MagentoService();
            String mlogin = mservice.login("admin", "Interconnection123!");
            catalogInventoryStockItemUpdateEntity qty_update = new catalogInventoryStockItemUpdateEntity();
            catalogProductCreateEntity create = new catalogProductCreateEntity();
            catalogCategoryEntity add_cat = new catalogCategoryEntity();
            string header = "<div class='ui items'>";
            string footer = "</div>";
            spec.desc = header + spec.cpu_desc + spec.ram_desc + spec.hdd_desc + spec.ic_cert + footer;
            
            string[] website_id = { "1" };
            create.description = spec.desc;
            create.short_description = spec.short_desc;
            create.price = "0";
            create.status = "2"; //set to disable

            create.visibility = "4"; //set to not Search, Catalog
            create.weight = "8.0";
            create.website_ids = website_id; //currently set to 1 (main website)
            create.tax_class_id = "2";
            // create.name = spec.brand + " " + spec.model + " (" + spec.cpu + "," + spec.ram + "GB RAM," + spec.hdd + " GB HDD)";
            create.name = spec.listing_title;
            create.meta_title = spec.meta_title;
            create.meta_description = spec.meta_desc;

            associativeEntity[] attributes = new associativeEntity[17];
            attributes[0] = new associativeEntity();
            attributes[0].key = "cpu";
            attributes[0].value = spec.cpu;
            attributes[1] = new associativeEntity();
            attributes[1].key = "ram";
            attributes[1].value = spec.ram + "GB";
            attributes[2] = new associativeEntity();
            attributes[2].key = "hdd";
            attributes[2].value = spec.hdd + "GB";
            attributes[3] = new associativeEntity();
            attributes[3].key = "creation_date";
            attributes[3].value = spec.create_date;
            attributes[4] = new associativeEntity();
            attributes[4].key = "brand";
            attributes[4].value = spec.brand;
            attributes[5] = new associativeEntity();
            attributes[5].key = "incl";
            attributes[5].value = spec.accessories;
            attributes[6] = new associativeEntity();
            attributes[6].key = "os";
            attributes[6].value = spec.software;
            attributes[7] = new associativeEntity();
            attributes[7].key = "processor";
            attributes[7].value = spec.cpu_dropdown;
            attributes[8] = new associativeEntity();
            attributes[8].key = "memory";
            attributes[8].value = spec.memory_dropdown;
            attributes[9] = new associativeEntity();
            attributes[9].key = "harddrive";
            attributes[9].value = spec.hdd_dropdown;
            attributes[10] = new associativeEntity();
            attributes[10].key = "software_description";
            attributes[10].value = spec.soft_desc;
            attributes[11] = new associativeEntity();
            attributes[11].key = "computer_manufacturer";
            attributes[11].value = spec.brand_dropdown;
            attributes[12] = new associativeEntity();
            attributes[12].key = "vendor_id";
            attributes[12].value = "4";
            attributes[13] = new associativeEntity();
            attributes[13].key = "approval";
            attributes[13].value = "2";
            attributes[14] = new associativeEntity();
            attributes[14].key = "vendor_sku";
            attributes[14].value = spec.sku;
            attributes[15] = new associativeEntity();
            attributes[15].key = "wireless";
            attributes[15].value = spec.wireless;
            attributes[16] = new associativeEntity();
            attributes[16].key = "grade";
            attributes[16].value = spec.grade;

            catalogProductAdditionalAttributesEntity additionalAttributes = new catalogProductAdditionalAttributesEntity();
            additionalAttributes.single_data = attributes;
            create.additional_attributes = additionalAttributes;
            mservice.catalogProductCreate(mlogin, "simple", "4", spec.sku, create, "1");
            mservice.catalogCategoryAssignProduct(mlogin, 2, spec.sku, "1", "SKU");

            //update QTY of newly create product
            qty_update.manage_stockSpecified = true;
            qty_update.manage_stock = 1;
            qty_update.is_in_stock = 1;
            qty_update.is_in_stockSpecified = true;
            qty_update.qty = "0";
            mservice.catalogInventoryStockItemUpdate(mlogin, spec.sku, qty_update);
        }

       

        public RefrubHistoryObj listing_info(RefrubHistoryObj spec)
        {
            //format magento listing machine accsessories description
            switch (spec.type)
            {
                case "_DK":
                    spec.accessories = "Power Cable, Windows-based keyboard and mouse";
                    break;
                case "_LP":
                    spec.accessories = "AC Adapter";
                    break;
            }

            //format the meta description on magento listing 
            spec.meta_desc = "US based nonprofits and low-income individuals can get a great deal on a refurbished laptop right here on the InterConnection Online Store.  This is a Windows 10 machine that has been tested and IC Certified by our in-house technicians.";
            //format the listing title 
            string title_cpu = spec.cpu.Replace("Intel(R)", "");
            title_cpu = title_cpu.Replace("(TM)", " ");
            if (!title_cpu.Contains("AMD"))
            {
                if (title_cpu.Contains("CPU"))
                {
                    int posA = title_cpu.IndexOf("CPU");
                    int posB = title_cpu.LastIndexOf("@");
                    string between = title_cpu.Substring(posA, posB - posA);
                    title_cpu = title_cpu.Replace("CPU", "");
                }
            }
            title_cpu = title_cpu.Trim();
            spec.listing_title = spec.brand + " " + spec.model + " (" + title_cpu + ", " + spec.ram + "GB RAM, " + spec.hdd + "GB HDD)";
            if (spec.channel.Contains("Mar"))
            {
                spec.meta_title = spec.brand + " " + spec.model + " (" + spec.cpu + "," + spec.ram + "," + spec.hdd + ") with Microsoft Windows 10 Professional and Microsoft Office 2010 Home & Business.";
                if (spec.type == "_DK")
                {
                    spec.short_desc = "Get a great price on a great quality refurbished desktop right here at InterConnection. This desktop comes with a " + spec.cpu + " processor, " + spec.ram + "GB of Memory, " + spec.hdd + "GB Hard Drive, with Windows 10 Pro and Microsoft Office 2010 Home & Business.We back all of our products with a 1 year warranty.";
                }
                else
                {
                    spec.short_desc = "Get a great price on a great quality refurbished laptop right here at InterConnection. This laptop comes with a " + spec.cpu + " processor, " + spec.ram + "GB of Memory, " + spec.hdd + "GB Hard Drive, with Windows 10 Pro and Microsoft Office 2010 Home & Business.We back all of our products with a 1 year warranty.";
                }

                spec.ic_cert = "<div class='row'><div class='col-sm-8'><p><a href='http://interconnection.org/store/iccertified/'><img style='padding-left: 8px;' align='right' alt='IC Certified' src='http://connectall.org/media/wysiwyg/ICCertified.png' /></a>InterConnection (IC) is a leading provider of quality refurbished laptops and computers to nonprofit organizations and low-income individuals in the U.S. and to schools and non-governmental organizations around the world. We have developed a computer refurbishing process that ensures all computers pass a series of refurbishment stages and meet the highest quality control standards. All computers that meet our standards are deemed <a href='http://interconnection.org/store/iccertified/'>&ldquo;IC Certified&rdquo;</a>, which is our symbol for quality and reliability.</p></div></div></div>";


                spec.software = "Windows 10 Professional & Microsoft Office Home and Business 2010";
                spec.soft_desc = "<p>Windows 10 Professional, 64 bit</p><p>Microsoft Office 2010 Home and Business with Office, Excel, Word, Outlook and Power Point</p><p><strong>InterConnection exclusive:</strong>&nbsp;all of our computers come with a recovery partition, allowing users to restore the PCs operating system to like-new condition&nbsp;<em>without an installation disk.</em></p>";
                spec.pallet = spec.sku;

                if (spec.channel.Contains("OEM"))

                {
                    spec.meta_title = spec.brand + " " + spec.model + " (" + spec.cpu + "," + spec.ram + "," + spec.hdd + ") with Microsoft Windows 10 Professional ";
                    spec.sku = "OEM_" + spec.sku;
                    spec.software = "Windows 10 Professional";
                    spec.soft_desc = "<p>Windows 10 Professional, 64 bit</p><p><strong>InterConnection exclusive:</strong>&nbsp;all of our computers come with a recovery partition, allowing users to restore the PCs operating system to like-new condition&nbsp;<em>without an installation disk.</em></p>";
                }
                
            }
            return spec;
        }
    }

}