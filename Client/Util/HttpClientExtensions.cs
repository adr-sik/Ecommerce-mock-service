namespace Client.Util
{
    public static class HttpClientExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri)
            {
                Content = req.Content, // Note: This shallow copies the content reference
                Version = req.Version
            };

            // Copy all headers from the original request
            foreach (var header in req.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }

            // We must remove the Content headers from the main request, 
            // as they belong to the Content property
            if (clone.Content != null)
            {
                foreach (var header in req.Content.Headers)
                {
                    clone.Content.Headers.Add(header.Key, header.Value);
                }
            }

            // This is necessary if the original request had its content stream consumed
            if (req.Content is StreamContent streamContent)
            {
                // If the content is stream-based, we must rewind or create a new stream
                var originalStream = streamContent.ReadAsStream();
                if (originalStream.CanSeek)
                {
                    originalStream.Position = 0;
                }
                // For a robust implementation, especially if content is large, 
                // you might need a more complex stream copy/reset logic here.
                clone.Content = new StreamContent(originalStream);
                // Copy headers back to the new content object
                foreach (var header in req.Content.Headers)
                {
                    clone.Content.Headers.Add(header.Key, header.Value);
                }
            }

            return clone;
        }
    }
}
