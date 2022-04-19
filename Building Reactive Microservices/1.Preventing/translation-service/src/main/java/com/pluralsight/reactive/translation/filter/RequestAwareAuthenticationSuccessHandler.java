package com.pluralsight.reactive.translation.filter;

import org.springframework.security.core.Authentication;
import org.springframework.security.web.authentication.SimpleUrlAuthenticationSuccessHandler;
import org.springframework.security.web.savedrequest.HttpSessionRequestCache;
import org.springframework.security.web.savedrequest.RequestCache;
import org.springframework.security.web.savedrequest.SavedRequest;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@Component
public class RequestAwareAuthenticationSuccessHandler extends SimpleUrlAuthenticationSuccessHandler {

    private final RequestCache requestCache;

    public RequestAwareAuthenticationSuccessHandler() {
        this.requestCache = new HttpSessionRequestCache();
    }

    @Override
    public void onAuthenticationSuccess(final HttpServletRequest httpServletRequest,
                                        final HttpServletResponse httpServletResponse,
                                        final Authentication authentication) {
        final SavedRequest savedRequest = requestCache.getRequest(httpServletRequest, httpServletResponse);

        if (savedRequest == null) {
            clearAuthenticationAttributes(httpServletRequest);
            return;
        }

        final String targetUrlParameter = getTargetUrlParameter();
        final String parameter = httpServletRequest.getParameter(targetUrlParameter);

        if (isAlwaysUseDefaultTargetUrl() || (targetUrlParameter != null && StringUtils.hasText(parameter))) {
            requestCache.removeRequest(httpServletRequest, httpServletResponse);
            clearAuthenticationAttributes(httpServletRequest);
            return;
        }

        clearAuthenticationAttributes(httpServletRequest);
    }
}
