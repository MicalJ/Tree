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
        private readonly HttpClient _httpClient;

        public TreeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Default");
        }

        public async Task<string> BuildTreeAsync()
        {
            var nodes = await GetTreeAsync();

            return await Task.Run(() =>
            {
                if (nodes == null || nodes.Count == 0)
                {
                    return string.Empty;
                }

                return BuildTree(nodes);
            });
        }

        private string BuildTree(List<Node> nodes)
        {
            StringBuilder result = new StringBuilder();

            Node root = nodes.Where(w => w.ParentId == null).First();
            List<Node> children = nodes.Where(w => w.ParentId != null).ToList();

            result.Append($"<li class=\"treeNode\" id=\"liNode{root.Id}\"><span class=\"treeSwitcher\"></span>" +
                $"<span class=\"treeLabel\" id=\"root\" value=\"{root.Id}\">{root.Name}</span>" +
                $"<span class=\"btn btn-primary btn-sm\" onclick=\"addNodeInput({root.Id})\">Add</span>" +
                $"<span style=\"display:none\" id=\"addNodeSpan{root.Id}\">" +
                $"<input class=\"form-control col-sm-2\" id=\"addNodeName{root.Id}\" type=\"text\" placeholder=\"Type name\">" +
                $"<button class=\"btn btn-success btn-sm\" onclick=\"addNode({root.Id})\">Save</button></span>" +
                $"<ul class=\"treeNodes\">");

            for (int i = 0; i < children.Count; i++)
            {
                BuildNode(ref children, children[i], ref result,
                    nodes.Where(w => w.Id != children[i].Id).ToList());
            }

            result.AppendLine("</ul>");

            return result.ToString();
        }

        private async Task<List<Node>> GetTreeAsync()
        {
            var response = await _httpClient.GetAsync("node");

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Cannot build tree correct");
            }

            return JsonConvert.DeserializeObject<List<Node>>(responseContent);
        }

        private static void BuildNode(ref List<Node> children, Node child, ref StringBuilder result, List<Node> allNodes)
        {
            if (!children.Any(a => a.ParentId == child.Id))
            {
                FillNode(result, child, allNodes);

                return;
            }

            var subChildren = children.Where(w => w.ParentId == child.Id).ToList();
            children = children.Except(subChildren).ToList();

            FillNode(result, child, allNodes);

            result.Append("<ul class=\"treeNodes\">");

            foreach (var subChild in subChildren)
            {
                BuildNode(ref children, subChild, ref result, allNodes);
            }

            result.Append("</li>");
            result.Append("</ul>");
        }

        private static StringBuilder FillNode(StringBuilder result, Node child, List<Node> allNodes)
        {
            result.Append($"<li id=\"liNode{child.Id}\" class=\"treeNode\">" +
                   $"<span style=\"cursor:not-allowed\" class=\"treeSwitcher\"></span>" +
                   $"<span style=\"cursor:not-allowed\" class=\"treeLabel\" value=\"{child.Id}\">{child.Name}</span>" +
                   $"<span class=\"btn btn-primary btn-sm\" onclick=\"addNodeInput({child.Id})\"> Add</span>" +
                   $"<span class=\"btn btn-primary btn-sm\" onclick=\"editNodeInput({child.Id}, {child.ParentId})\"> Edit</span>" +
                   $"<span class=\"btn btn-danger btn-sm\" onclick=\"deleteNode({child.Id})\"> Delete</span>" +
                   $"<span style=\"display:none\" id=\"addNodeSpan{child.Id}\">" +
                   $"<input class=\"form-control col-sm-2\" id=\"addNodeName{child.Id}\" type=\"text\" placeholder=\"Type name\">" +
                   $"<button class=\"btn btn-success btn-sm\" onclick=\"addNode({child.Id})\">Save</button></span>" +
                   $"<span style=\"display:none;\" id=\"editNodeSpan{child.Id}\">" +
                   $"<input class=\"form-control col-sm-2\" id=\"editNodeName{child.Id}\" value=\"{child.Name}\" type=\"text\" placeholder=\"Type name\"\\>" +
                   $"<select class=\"form-control form-control-sm\" id=\"editNodeParentId{child.Id}\">");

            foreach (var allNode in allNodes)
            {
                result.Append($"<option  value=\"{allNode.Id}\">{allNode.Name}</option>");
            }

            result.Append($"</select>" +
                $"<button class=\"btn btn-success btn-sm\" onclick=\"updateNode({child.Id})\">Save</button></span>" +
                $"</li>");

            return result;
        }
    }
}
