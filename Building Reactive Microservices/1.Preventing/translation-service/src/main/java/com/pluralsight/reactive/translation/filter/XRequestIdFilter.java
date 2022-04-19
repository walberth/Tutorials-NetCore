package com.pluralsight.reactive.translation.filter;

import brave.Tracer;
import com.pluralsight.reactive.translation.constant.Headers;
import org.springframework.beans.factory.annotation.Autowired;

import javax.servlet.*;
import javax.servlet.annotation.WebFilter;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

@WebFilter("/**")
public class XRequestIdFilter implements Filter {

    @Autowired
    private Tracer tracer;

    @Override
    public void doFilter(final ServletRequest servletRequest,
                         final ServletResponse servletResponse,
                         final FilterChain filterChain) throws IOException, ServletException {
        final HttpServletResponse httpServletResponse = (HttpServletResponse) servletResponse;

        httpServletResponse.setHeader(Headers.X_REQUEST_ID, tracer.currentSpan().context().traceIdString());

        filterChain.doFilter(servletRequest, servletResponse);
    }
}
