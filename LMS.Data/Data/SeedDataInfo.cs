using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    class SeedDataInfo
    {
        public static readonly Dictionary<string, string> CourseNamePool =
            new() /* Name - Description pair */
            {
                {
                    "Android Course",
                    "Expand your mobile app reach through this Android application development and programming training. Android's open source platform offers compatibility with a wide range of devices, which provide global access to the mobile market."
                },
                {
                    "Big Data",
                    "The course gives an overview of the Big Data phenomenon, focusing then on extracting value from the Big Data using predictive analytics techniques."
                },
                {
                    "C# Backend",
                    "This is an introductory programming course using the C# language. It does not assume any prior programming experience. This course will prepare students for intermediate C# and ASP.NET courses."
                },
                {
                    "Cloud Security",
                    "The course leverages cloud computing security guidelines set forth by the International Organization for Standardization (ISO), National Institute of Standards and Technology (NIST), European Union Agency for Network and Information Security (ENISA), and Cloud Security Alliance (CSA). This course reviews security characteristics of leading cloud infrastructure providers and applied deployment scenarios with the internet of things (IoT) and blockchain."
                },
                {
                    "Computer Security Analyst",
                    "The security analyst plays a vital role in keeping an organization’s proprietary and sensitive information secure. He/she works inter-departmentally to identify and correct flaws in the company’s security systems, solutions, and programs while recommending specific measures that can improve the company’s overall security posture."
                },
                {
                    "Cybersecurity",
                    "This introductory certification course is the fastest way to get up to speed in information security. Written and taught by battle-scarred security veterans, this entry-level course covers a broad spectrum of security topics and is liberally sprinkled with real life examples. A balanced mix of technical and managerial issues makes this course appealing to attendees who need to understand the salient facets of information security basics and the basics of risk management.  "
                },
                {
                    "Digitization of the Legal Sector",
                    "During the 13-month course, participants will be exposed to the expertise of both schools, which are leaders in their respective disciplines, namely, technology and law. And after completing the course, participants will be equipped with the knowledge and skills they need to make a difference in the field of technology law."
                },
                {
                    "Embedded Dev",
                    "Welcome to the Introduction to Embedded Systems Software and Development Environments. This course is focused on giving you real world coding experience and hands on project work with ARM based Microcontrollers. You will learn how to implement software configuration management and develop embedded software applications. Course assignments include creating a build system using the GNU Toolchain GCC, using Git version control, and developing software in Linux on a Virtual Machine. The course concludes with a project where you will create your own build system and firmware that can manipulate memory."
                },
                {
                    "Self-Paced Programs",
                    "Self-paced learning enables employees to create their schedules. It is especially helpful for participants that have other tasks and can’t attend a live class or training. Even if there is a deadline to complete a course, for example, they can choose how and when they take it."
                },
                {
                    "Web Design",
                    "This Specialization covers how to write syntactically correct HTML5 and CSS3, and how to create interactive web experiences with JavaScript. Mastering this range of technologies will allow you to develop high quality web sites that, work seamlessly on mobile, tablet, and large screen browsers accessible. During the capstone you will develop a professional-quality web portfolio demonstrating your growth as a web developer and your knowledge of accessible web design. This will include your ability to design and implement a responsive site that utilizes tools to create a site that is accessible to a wide audience, including those with visual, audial, physical, and cognitive impairments."
                }
            };

        public static readonly Dictionary<string, string> ModuleNamePool = new()
        {
            {
                "Android Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Android Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Big Data Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Big Data Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics and prescriptive analytics, and apply these concepts to propose solutions in Big Data cases."
            },

            {
                "C# Backend Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "C# Backend Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Cloud Security Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cloud Security Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Computer Security Analyst Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Computer Security Analyst Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Cybersecurity Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cybersecurity Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Digitization of the Legal Sector Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Digitization of the Legal Sector Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Embedded Dev Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Embedded Dev Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Self-Paced Programs Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Self-Paced Programs Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Web Design Module 1",
                "ModuleDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Web Design Module 2",
                "ModuleDescription2 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            }
        };

        public static readonly Dictionary<string, string> ActivityNamePool = new()
        {
            {
                "Android Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Android Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Android Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Android Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Big Data Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Big Data Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Big Data Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Big Data Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "C# Backend Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "C# Backend Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "C# Backend Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "C# Backend Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Cloud Security Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cloud Security Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cloud Security Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cloud Security Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Computer Security Analyst Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Computer Security Analyst Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Computer Security Analyst Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Computer Security Analyst Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Cybersecurity Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cybersecurity Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cybersecurity Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Cybersecurity Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Digitization of the Legal Sector Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Digitization of the Legal Sector Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Digitization of the Legal Sector Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Digitization of the Legal Sector Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Embedded Dev Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Embedded Dev Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Embedded Dev Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Embedded Dev Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Self-Paced Programs Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Self-Paced Programs Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Self-Paced Programs Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Self-Paced Programs Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },

            {
                "Web Design Activity 1",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Web Design Activity 2",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Web Design Activity 3",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
            {
                "Web Design Activity 4",
                "ActivityDescription1 After completing this course, students can understand the concepts of descriptive analytics, predictive analytics "
            },
        };

        public static readonly List<string> StudentNamePool = new()
        {
            { "Lucas" },
            { "Liam" },
            { "William" },
            { "Elias" },
            { "Noah" },
            { "Hugo" },
            { "Oliver" },
            { "Oscar" },
            { "Adam" },
            { "Matt" },
            { "Lars" },
            { "Mikael" },
            { "Anders" },
            { "Erik" },
            { "Per" },
            { "Karl" },
            { "Peter" },
            { "Thomas" },
            { "Jan" },
            { "Ola" },
            { "Gustaf" },
            { "Sven" },
            { "Nils" },
            { "Alexander" },
            { "Vincent" },
            { "Theo" },
            { "Isak" },
            { "Arvid" },
            { "August" },
            { "Ludvig" }
        };
    }
}
