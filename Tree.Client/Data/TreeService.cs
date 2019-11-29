using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tree.Client.Data
{
    public class TreeService
    {
        private static string character = "-";
        private readonly HttpClient _httpClient;

        public TreeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Default");
        }

        public async Task<string> BuildTreeAsync()
        {
    //        List<Node> nodes = new List<Node>
    //{
    //           new Node
    //           {
    //               Id=1,
    //               Name="Root"
    //           },
    //           new Node
    //           {
    //               Id=2,
    //               Name="child1Root",
    //               ParentId=1
    //           },
    //           new Node
    //           {
    //               Id=7,
    //               Name="child1child1Root",
    //               ParentId=2
    //           },
    //           new Node
    //           {
    //               Id=3,
    //               Name="child2Root",
    //               ParentId=1
    //           },
    //           new Node
    //           {
    //               Id=4,
    //               Name="child3Root",
    //               ParentId=1
    //           },
    //              new Node
    //           {
    //               Id=9,
    //               Name="child1child1child1child3Root",
    //               ParentId=6
    //           },
    //           new Node
    //           {
    //               Id=5,
    //               Name="child1child3Root",
    //               ParentId =4
    //           },
    //           new Node
    //           {
    //               Id=6,
    //               Name="child1child1child3Root",
    //               ParentId=5
    //           },
    //              new Node
    //           {
    //               Id=8,
    //               Name="child2child1child3Root",
    //               ParentId=5
    //           }
    //        };
            StringBuilder result = new StringBuilder();

            var response = await _httpClient.GetAsync("node");

            string responseContent = await response.Content.ReadAsStringAsync();

            var nodes = JsonConvert.DeserializeObject<List<Node>>(responseContent);

            return await Task.Run(() =>
            {
                if (nodes == null || nodes.Count == 0)
                {
                    return string.Empty;
                }

                Node root = nodes.Where(w => w.ParentId == null).First();
                List<Node> children = nodes.Where(w => w.ParentId != null).ToList();

                result.AppendLine($"<td>{root.Name}</td>");

                for (int i = 0; i < children.Count; i++)
                {
                    character = "-";

                    result.Append(character);
                    BuildNode(ref children, children[i], ref result);
                }

                return result.ToString();
            });
        }

        private static void BuildNode(ref List<Node> children, Node child, ref StringBuilder result)
        {
            result.AppendLine($"<td>{child.Name} - {child.Id}</td>");

            if (!children.Any(a => a.ParentId == child.Id))
            {
                return;
            }

            var subChildren = children.Where(w => w.ParentId == child.Id).ToList();
            children = children.Except(subChildren).ToList();
            character += '-';

            foreach (var subChild in subChildren)
            {
                result.Append(character);
                BuildNode(ref children, subChild, ref result);
            }

            character = character.Remove(character.Length - 1);
        }
    }
}
